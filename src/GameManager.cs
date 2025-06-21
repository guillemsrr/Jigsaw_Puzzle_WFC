// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Linq;
using Godot;
using GodotTest.Puzzle;
using GodotTest.WFC.Generation;
using GodotTest.WFC.Utilities;

namespace GodotTest;

public partial class GameManager : Node
{
	[Export] private LevelGenerator _generator;
	[Export] private Camera3D _camera;
	[Export] private Texture2D _puzzleTexture;
	[Export] private FileDialog _fileDialog;
	[Export] private float _solveAnimationDuration = 3f;

	public float GetSolveAnimationTime()
	{
		return _solveAnimationDuration;
	}

	public enum PuzzleGenerationType
	{
		Solved,
		AnimatedSolve,
		Randomized
	}

	PuzzleGenerationType _generationType = PuzzleGenerationType.Solved;

	public override void _Ready()
	{
		_generator.Initialize();
		Generate();

		_fileDialog.FileSelected += OnFileSelected;
	}

	public void SetOption(int option)
	{
		_generationType = (PuzzleGenerationType) option;
	}

	public void Generate()
	{
		_generator.PrepareGeneration();
		_generator.GenerateLevel();

		ApplyMaterial();

		UpdateCameraPosition();
		UpdateCameraParameters();

		if (_generationType == PuzzleGenerationType.Randomized)
		{
			RandomizePuzzlePieces();
		}
		else if (_generationType == PuzzleGenerationType.AnimatedSolve)
		{
			SolvePuzzleAnimated();
			RandomizePuzzlePieces();
		}
	}

	public void SolvePuzzleAnimated()
	{
		var tween = CreateTween().SetParallel();
		tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.InOut);

		foreach (var module in _generator.InstancedModules.Values)
		{
			var piece = (PuzzlePiece) module;

			tween.TweenProperty(piece, "global_position", piece.PuzzlePosition, _solveAnimationDuration);
			tween.TweenProperty(piece, "quaternion", piece.PuzzleRotation, _solveAnimationDuration);
		}
	}

	public void RandomizePuzzlePieces()
	{
		var rng = new RandomNumberGenerator();
		float radius = Mathf.Max(_generator.GridDimensions.X, _generator.GridDimensions.Y) * _generator.ModuleSize *
					   0.5f;
		float boardCenterX = _generator.GridDimensions.X * _generator.ModuleSize / 2f;
		float boardCenterZ = _generator.GridDimensions.Y * _generator.ModuleSize / 2f;

		foreach (var module in _generator.InstancedModules.Values)
		{
			var piece = (PuzzlePiece) module;

			// Randomize position in a circular area around the center of the board
			float angle = rng.RandfRange(0, Mathf.Pi * 2);
			float dist = rng.RandfRange(radius * 0.7f, radius * 1.5f);
			var randomPosition = new Vector3(boardCenterX + dist * Mathf.Cos(angle), piece.GlobalPosition.Y,
				boardCenterZ + dist * Mathf.Sin(angle));
			piece.SetGlobalPosition(randomPosition);

			var rotations = Rotations.QuaternionByRotation.Values.ToArray();
			var randomRotation = rotations[rng.RandiRange(0, rotations.Length - 1)];
			piece.SetQuaternion(randomRotation);
		}
	}

	private void OnFileSelected(string path)
	{
		var image = Image.LoadFromFile(path);
		if (image != null)
		{
			_puzzleTexture = ImageTexture.CreateFromImage(image);
			ApplyMaterial();
		}
	}

	private void UpdateCameraPosition()
	{
		float centerX = _generator.GridDimensions.X * _generator.ModuleSize / 3f;
		float centerZ = _generator.GridDimensions.Y * _generator.ModuleSize / 3f;
		float height = 100f;

		_camera.Position = new Vector3(centerX, height, centerZ);
	}

	private void UpdateCameraParameters()
	{
		float size = Mathf.Max(_generator.GridDimensions.X * _generator.ModuleSize,
			_generator.GridDimensions.Y * _generator.ModuleSize);
		_camera.Size = size * 2f;
	}

	private void ApplyMaterial()
	{
		int columns = _generator.GridDimensions.X;
		int rows = _generator.GridDimensions.Y;

		Vector2 pieceSize = new Vector2(1.0f / columns, 1.0f / rows);

		foreach (var generatorInstancedModule in _generator.InstancedModules)
		{
			Vector3I pos = generatorInstancedModule.Key;
			PuzzlePiece piece = (PuzzlePiece) generatorInstancedModule.Value;
			ShaderMaterial shaderMaterial = piece.PuzzleVisual.GetShader();

			Vector2 uvRegionOrigin = new Vector2(pos.X * pieceSize.X, pos.Z * pieceSize.Y);

			float angle = piece.GlobalRotation.Y;

			shaderMaterial.SetShaderParameter("uv_offset", uvRegionOrigin);
			shaderMaterial.SetShaderParameter("uv_scale", pieceSize);
			shaderMaterial.SetShaderParameter("uv_rotation", angle);
			shaderMaterial.SetShaderParameter("albedo_texture", _puzzleTexture);

			piece.PuzzleVisual.SetMaterialOverride(shaderMaterial);
		}
	}
}
