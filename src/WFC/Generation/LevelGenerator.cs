using System;
using System.Collections.Generic;
using Godot;
using GodotTest.Puzzle;
using GodotTest.WFC.Adjacency;
using GodotTest.WFC.Generation.Cells;
using GodotTest.WFC.Generation.Constraints;
using GodotTest.WFC.Generation.Waves;
using GodotTest.WFC.Modules;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Generation
{
	public partial class LevelGenerator : Node3D
	{
		private const int MAX_OBSERVATION_TRIES = 0;

		[Export] public PackedScene[] PiecePrefabs;
		[Export] private Node3D _generationParent;
		[Export] public float ModuleSize = 2f;
		[Export] private ulong _seed = 0;
		[Export] private bool _randomSeed = true;
		private RandomNumberGenerator _rng = new RandomNumberGenerator();

		public Vector2I GridDimensions { get; set; } = new Vector2I(5, 5);

		private List<ModuleData> _modulesData = new List<ModuleData>();
		private Wave _wave = new Wave();
		private WaveFunctionCollapse _waveFunctionCollapse;
		private FrequencyController _frequencyController;
		private List<ConstraintApplier> _constraints;
		private int _observationTries;

		public Dictionary<Vector3I, Node3D> InstancedModules = new Dictionary<Vector3I, Node3D>();

		private ModuleRotationChecker _moduleRotationChecker = new ModuleRotationChecker();

		public void Initialize()
		{
			_modulesData = ExtractDataFromModules();
		}

		public void PrepareGeneration()
		{
			CreateCells();
			InitializeSubClasses();
			ApplyConstraints();
		}

		private List<ModuleData> ExtractDataFromModules()
		{
			int numberModuleData = 0;
			List<ModuleData> moduleDatas = new List<ModuleData>();
			foreach (PackedScene packedScene in PiecePrefabs)
			{
				PuzzlePiece piece = packedScene.Instantiate<PuzzlePiece>();
				AdjacencyData adjacencyData = piece.AdjacencyData;
				piece.QueueFree();

				List<Rotation> differentRotations = _moduleRotationChecker.GetAllDifferentRotations(adjacencyData);
				foreach (Rotation rotation in differentRotations)
				{
					ModuleData moduleData = new ModuleData(packedScene, adjacencyData, rotation, numberModuleData);
					moduleDatas.Add(moduleData);

					numberModuleData++;
				}
			}

			return moduleDatas;
		}

		public void GenerateLevel()
		{
			foreach (var keyValuePair in InstancedModules)
			{
				keyValuePair.Value.QueueFree();
			}

			SetRandom();

			if (!Observe())
			{
				_observationTries++;
				if (_observationTries < MAX_OBSERVATION_TRIES)
				{
					GD.PrintErr("Resetting observation");

					Reset();
					GenerateLevel();
					return;
				}

				GD.PrintErr("Couldn't generate");
				return;
			}

			DrawCells();
		}

		private bool Observe()
		{
			bool observation = _waveFunctionCollapse.Observe();

			//TODO: if observation is not suited
			/*do
			{
				_waveFunctionCollapse.Observe();
				_observationTries++;
			}
			while (_observationTries < MAX_OBSERVATION_TRIES);*/

			return observation;
		}

		private void SetRandom()
		{
			if (_randomSeed)
			{
				_seed = (ulong) Random.Shared.NextInt64();
			}

			GD.Seed(_seed);
		}

		private void DrawCells()
		{
			InstancedModules = new Dictionary<Vector3I, Node3D>();
			foreach (CellController cell in _wave.Cells.Values)
			{
				if (!cell.IsCollapsed)
				{
					GD.PrintErr("Trying to draw uncollapsed cell");
					continue;
				}

				Node3D module = cell.InstantiateModule();
				var piece = (PuzzlePiece) module;
				piece.SetCurrentAsPuzzleSolution();
				InstancedModules.Add(cell.Position, module);
			}
		}

		private void ApplyConstraints()
		{
			foreach (ConstraintApplier constraintApplier in _constraints)
			{
				constraintApplier.ApplyConstraint(_wave.Cells);
			}
		}

		private void InitializeSubClasses()
		{
			_constraints = new List<ConstraintApplier>()
			{
				new PerimeterConstraint(new Vector3I(GridDimensions.X, 1, GridDimensions.Y))
			};

			_frequencyController = new FrequencyController();
			_frequencyController.CalculateInitialWeight(_wave.Cells.Values);
			_waveFunctionCollapse = new WaveFunctionCollapse(_wave);
		}

		private void CreateCells()
		{
			_wave = new Wave();

			for (int x = 0; x < GridDimensions.X; x++)
			{
				for (int y = 0; y < GridDimensions.Y; y++)
				{
					CreateCell(new Vector3I(x, 0, y));
				}
			}
		}

		private void CreateCell(Vector3I position)
		{
			CellController cellController =
				new CellController(_generationParent, _modulesData.ToArray(), position, ModuleSize);
			_wave.Cells[position] = cellController;
		}

		private void Reset()
		{
			/*while (_generationParent.childCount != 0)
			{
				DestroyImmediate(_generationParent.GetChild(0).gameObject);
			}*/

			_wave.Cells.Clear();
		}
	}
}
