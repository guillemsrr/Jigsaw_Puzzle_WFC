// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;
using Godot;
using GodotTest.Puzzle;
using GodotTest.WFC.Adjacency;
using GodotTest.WFC.Modules;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Test;

public partial class TestNode : Node3D
{
    [Export] private PackedScene packedScene;

    private ModuleRotationChecker _moduleRotationChecker = new ModuleRotationChecker();

    public override void _Ready()
    {
        base._Ready();

        //Test();
    }

    private void Test()
    {
        List<ModuleData> moduleDatas = new List<ModuleData>();
        PuzzlePiece piece = packedScene.Instantiate<PuzzlePiece>();
        AdjacencyData adjacencyData = piece.AdjacencyData;
        piece.QueueFree();

        List<Rotation> differentRotations = _moduleRotationChecker.GetAllDifferentRotations(adjacencyData);
        foreach (Rotation rotation in differentRotations)
        {
            ModuleData moduleData = new ModuleData(packedScene, adjacencyData, rotation);
            moduleDatas.Add(moduleData);
        }

        Vector3 position = new Vector3(0, 0, 0);
        foreach (ModuleData moduleData in moduleDatas)
        {
            foreach (Direction direction in Directions.HorizontalDirections)
            {
                Rotation Rot = moduleData.Rotation;
                Direction rotatedDirection = Directions.RotateDirection(direction, Rot);

                AdjacencyType adjacencyType = moduleData.AdjacencyData.FacesByDirection[direction];
                AdjacencyType rotatedAdjacencyType = moduleData.AdjacencyData.FacesByDirection[rotatedDirection];
                int x = 0;
            }

            position += new Vector3(3f, 0, 0);

            Node3D moduleInstance = moduleData.PackedScene.Instantiate<Node3D>();
            AddChild(moduleInstance);

            Quaternion rotation = Rotations.QuaternionByRotation[moduleData.Rotation];
            moduleInstance.SetQuaternion(rotation);
            moduleInstance.SetPosition(position);
        }
    }
}