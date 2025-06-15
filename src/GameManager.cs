// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;
using LevelGenerator = GodotTest.WFC.Generation.LevelGenerator;

namespace GodotTest;

public partial class GameManager : Node
{
    [Export] private LevelGenerator _generator;
    [Export] private Camera3D _camera;

    public override void _Ready()
    {
        _generator.Initialize();
        _generator.GenerateLevel();

        UpdateCameraPosition();
        UpdateCameraParameters();
    }

    private void UpdateCameraPosition()
    {
        float centerX = 0f;
        float centerZ = _generator.GridDimensions.Y * _generator.ModuleSize / 2f;
        float height = 100f;

        _camera.Position = new Vector3(centerX, height, centerZ);
    }

    private void UpdateCameraParameters()
    {
        float size = Mathf.Max(_generator.GridDimensions.X * _generator.ModuleSize,
            _generator.GridDimensions.Y * _generator.ModuleSize);
        _camera.Size = size * 1.2f;
    }
}