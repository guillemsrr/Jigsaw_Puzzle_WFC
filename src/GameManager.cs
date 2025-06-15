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
    }
}