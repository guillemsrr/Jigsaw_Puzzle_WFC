// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Adjacency
{
    public partial class AdjacencyData : Node
    {
        [Export] public PuzzleAdjacencyResource Top; //-Z
        [Export] public PuzzleAdjacencyResource Down; //Z+
        [Export] public PuzzleAdjacencyResource Right; //X+
        [Export] public PuzzleAdjacencyResource Left; //X-

        public Dictionary<Direction, AdjacencyType> FacesByDirection { get; private set; }

        public void InitializeDirectionsDictionary()
        {
            FacesByDirection = new Dictionary<Direction, AdjacencyType>
            {
                [Direction.Front] = Top.Type,
                [Direction.Back] = Down.Type,
                [Direction.Left] = Left.Type,
                [Direction.Right] = Right.Type,
            };
        }
    }
}