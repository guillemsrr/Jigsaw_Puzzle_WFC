using System;
using System.Collections.Generic;
using Godot;

namespace GodotTest.WFC.Utilities
{
    public enum Direction
    {
        Left = 0,
        Down = 1,
        Back = 2,
        Right = 3,
        Up = 4,
        Front = 5
    }

    public class Directions
    {
        public static Dictionary<Direction, Vector3I> DirectionsByVectors = new Dictionary<Direction, Vector3I>
        {
            [Direction.Down] = Vector3I.Down,
            [Direction.Up] = Vector3I.Up,
            [Direction.Left] = Vector3I.Left,
            [Direction.Right] = Vector3I.Right,
            [Direction.Front] = Vector3I.Forward,
            [Direction.Back] = Vector3I.Back
        };

        public static Direction[] HorizontalDirections =
        {
            Direction.Front,
            Direction.Left,
            Direction.Back,
            Direction.Right
        };

        public static Direction RotateDirection(Direction direction, Rotation rotation)
        {
            if (IsVertical(direction))
            {
                return direction;
            }

            int index =
                (Array.IndexOf(HorizontalDirections, direction) + (int) rotation + HorizontalDirections.Length) %
                HorizontalDirections.Length;
            return HorizontalDirections[index % HorizontalDirections.Length];
        }

        public static Direction FlipDirection(Direction direction)
        {
            return direction switch
            {
                Direction.Left => Direction.Right,
                Direction.Right => Direction.Left,
                Direction.Up => Direction.Down,
                Direction.Down => Direction.Up,
                Direction.Front => Direction.Back,
                Direction.Back => Direction.Front,
                _ => direction
            };
        }

        public static bool IsVertical(Direction direction)
        {
            return direction == Direction.Up || direction == Direction.Down;
        }
    }
}