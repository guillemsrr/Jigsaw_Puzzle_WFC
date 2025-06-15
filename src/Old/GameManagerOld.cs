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
        if (_puzzleGenerator == null || _camera == null) return;

        float centerX = _puzzleGenerator.Columns * _puzzleGenerator.PieceSize / 2f;
        float centerZ = _puzzleGenerator.Rows * _puzzleGenerator.PieceSize / 2f;
        float height = Mathf.Max(_puzzleGenerator.Columns, _puzzleGenerator.Rows) * _puzzleGenerator.PieceSize * 2f;

        _camera.Position = new Vector3(centerX, height, centerZ + height);
        _camera.LookAt(new Vector3(centerX, 0, centerZ), Vector3.Up);
    }

    private void UpdateCameraParameters()
    {
        if (_puzzleGenerator == null || _camera == null) return;

        float size = Mathf.Max(_puzzleGenerator.Columns * _puzzleGenerator.PieceSize,
            _puzzleGenerator.Rows * _puzzleGenerator.PieceSize);
        _camera.Size = size * 1.2f;
    }
}