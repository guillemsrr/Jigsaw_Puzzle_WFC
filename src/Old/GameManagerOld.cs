// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;

namespace GodotTest.Old;

public partial class GameManagerOld : Node
{
    [Export] private PuzzleGenerator _puzzleGenerator;
    [Export] private Camera3D _camera;

    public override void _Ready()
    {
        _puzzleGenerator.Generate();

        UpdateCameraPosition();
        UpdateCameraParameters();
    }

    private void UpdateCameraPosition()
    {
        float centerX = 0f;
        float centerZ = _puzzleGenerator.Rows * _puzzleGenerator.PieceSize / 2f;
        float height = 100f;

        _camera.Position = new Vector3(centerX, height, centerZ + height);
    }

    private void UpdateCameraParameters()
    {
        float size = Mathf.Max(_puzzleGenerator.Columns * _puzzleGenerator.PieceSize,
            _puzzleGenerator.Rows * _puzzleGenerator.PieceSize);
        _camera.Size = size * 1.2f;
    }
}