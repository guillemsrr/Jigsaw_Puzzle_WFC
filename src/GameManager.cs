// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;
using GodotTest.Puzzle;
using GodotTest.WFC.Generation;

namespace GodotTest;

public partial class GameManager : Node
{
    [Export] private LevelGenerator _generator;
    [Export] private Camera3D _camera;
    [Export] private Texture2D _puzzleTexture;

    public override void _Ready()
    {
        _generator.Initialize();
        _generator.GenerateLevel();

        ApplyMaterial();

        UpdateCameraPosition();
        UpdateCameraParameters();
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