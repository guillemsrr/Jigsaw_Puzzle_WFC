using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Generation.Cells;
using GodotTest.WFC.Utilities;
using GodotTest.WFC.Modules;

namespace GodotTest.WFC.Generation.Constraints
{
    public class PerimeterConstraint : ConstraintApplier
    {
        private Vector3I _gridDimensions;

        public PerimeterConstraint(Vector3I gridDimensions)
        {
            _gridDimensions = gridDimensions;
        }

        public void ApplyConstraint(Dictionary<Vector3I, CellController> cells)
        {
            foreach (KeyValuePair<Vector3I, CellController> positionCell in cells)
            {
                AxisConstraint(positionCell.Value, positionCell.Key.X, _gridDimensions.X, Direction.Right);
                //AxisConstraint(positionCell.Value, positionCell.Key.Y, _gridDimensions.Y, Direction.Down);
                AxisConstraint(positionCell.Value, positionCell.Key.Z, _gridDimensions.Z, Direction.Back);
            }
        }

        private void AxisConstraint(CellController cell, int position, int dimension, Direction direction)
        {
            Direction oppositeDirection = Directions.FlipDirection(direction);
            if (position == 0)
            {
                RemoveModulesWithoutFlat(cell, oppositeDirection);
            }

            if (position == dimension - 1)
            {
                RemoveModulesWithoutFlat(cell, direction);
            }
        }

        private void RemoveModulesWithoutFlat(CellController cell, Direction direction)
        {
            List<ModuleData> toRemove = new List<ModuleData>();

            foreach (ModuleData module in cell.CellData.PossibleModules)
            {
                if (!HasFlatSide(module, direction))
                {
                    toRemove.Add(module);
                }
            }

            if (toRemove.Count == 0)
            {
                return;
            }

            foreach (ModuleData module in toRemove)
            {
                cell.CellData.PossibleModules.Remove(module);
            }

            if (!cell.HasPossibilities)
            {
                GD.PrintErr(
                    $"[PerimeterConstraint] Cell at {cell.Position} has no possible modules after removing");
            }
        }

        private bool HasFlatSide(ModuleData module, Direction direction)
        {
            Rotation rotation = module.Rotation;
            Direction rotatedDirection = Directions.RotateDirection(direction, rotation);

            AdjacencyType adjacency = module.AdjacencyData.FacesByDirection[rotatedDirection];
            bool isFlat = adjacency == AdjacencyType.Flat;
            if (!isFlat)
            {
                int x = 1;
            }

            return isFlat;
        }
    }
}