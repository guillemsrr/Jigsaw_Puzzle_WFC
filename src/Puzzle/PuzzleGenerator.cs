// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Godot;

namespace GodotTest.Puzzle._3d
{
	public partial class PuzzleGenerator : Node3D
	{
		[Export] public PackedScene PuzzlePieceScene;
		[Export] public Texture2D PuzzleTexture;
		[Export] public int Rows;
		[Export] public int Cols;
		[Export] public float ScatterRadius = 2f;

		public override void _Ready()
		{
			GD.Randomize();
			var pieces = new List<PuzzlePiece>();
			Vector2 pieceSize = new Vector2(1,1) / new Vector2(Cols, Rows);

			for (int row = 0; row < Rows; row++)
			{
				for (int col = 0; col < Cols; col++)
				{
					// Instantiate piece
					var piece = PuzzlePieceScene.Instantiate<PuzzlePiece>();
					piece.Row = row;
					piece.Col = col;

					// UV region in pixels
					piece.UVRegionOrigin = new Vector2(col * pieceSize.X, row * pieceSize.Y);
					piece.UVRegionSize = pieceSize;
					piece.Init();

					Vector3 testPos = new Vector3(col, 0f, row) * ScatterRadius;
					piece.SetGlobalPosition(testPos);

					AddChild(piece);
					pieces.Add(piece);
				}
			}
		}
	}
}