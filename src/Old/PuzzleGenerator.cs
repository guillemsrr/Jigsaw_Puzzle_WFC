// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotTest.Puzzle;

namespace GodotTest.Old
{
    public partial class PuzzleGenerator : Node3D
    {
        [Export] public PackedScene[] PuzzlePieces;
        [Export] public Texture2D PuzzleTexture;
        [Export] public int Rows = 2;
        [Export] public int Columns = 2;
        [Export] public float PieceSize = 2f;

        public void Generate()
        {
            GD.Randomize();

            List<int>[,] wave = { };
            var wfc = new WaveFunctionCollapseHandler(Rows, Columns, PuzzlePieces);
            if (wfc.Solve())
            {
                wave = wfc.Wave;
            }
            else
            {
                GD.PrintErr("error");
                return;
            }

            var instancedPieces = new List<PuzzlePiece>();
            Vector2 pieceSize = new Vector2(1, 1) / new Vector2(Columns, Rows);

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    var pieceModel = wave[col, row].First();
                    var piece = PuzzlePieces[pieceModel].Instantiate<PuzzlePiece>();
                    piece.Row = row;
                    piece.Col = col;

                    piece.UVRegionOrigin = new Vector2(col * pieceSize.X, row * pieceSize.Y);
                    piece.UVRegionSize = pieceSize;
                    piece.Init();

                    Vector3 testPos = new Vector3(col, 0f, row) * PieceSize;
                    piece.SetGlobalPosition(testPos);

                    AddChild(piece);
                    instancedPieces.Add(piece);
                }
            }
        }
    }
}