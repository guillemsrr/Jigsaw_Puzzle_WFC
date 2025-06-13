// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;
using GodotTest.Drag;

namespace GodotTest.Puzzle._3d;

public partial class PuzzlePiece : Node3D
{
	[Export] public int Row;
	[Export] public int Col;

	[Export] public Vector2 UVRegionOrigin;
	[Export] public Vector2 UVRegionSize;
	[Export] public PuzzleVisual PuzzleVisual;

	public void Init()
	{
		if (PuzzleVisual != null)
		{
			PuzzleVisual.UpdateUV(this);
		}
	}
}