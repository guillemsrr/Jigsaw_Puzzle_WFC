using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Generation.Cells;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Generation.Constraints
{
    public class PerimeterConstraint: ConstraintApplier
    {
        private int[] _perimeterModuleNumber;
        private Vector3I _gridDimensions;
        
        public PerimeterConstraint(int[] perimeterModuleNumber, Vector3I gridDimensions)
        {
            _perimeterModuleNumber = perimeterModuleNumber;
            _gridDimensions = gridDimensions;
        }

        public void ApplyConstraint(Dictionary<Vector3I, CellController> cells)
        {
            foreach (KeyValuePair<Vector3I,CellController> positionCell in cells)
            {
                AxisConstraint(positionCell.Value, positionCell.Key.X, _gridDimensions.X, Direction.Left);
                AxisConstraint(positionCell.Value, positionCell.Key.Y, _gridDimensions.Y, Direction.Down);
                AxisConstraint(positionCell.Value, positionCell.Key.Z, _gridDimensions.Z, Direction.Back);
            }
        }

        private void AxisConstraint(CellController cell, int position, int dimension, Direction direction)
        {
            if (position == 0)
            {
                foreach (int id in _perimeterModuleNumber)
                {
                    //cell.Propagate(Directions.FlipDirection(direction), id);
                }

            }

            if (position == dimension - 1)
            {
                foreach (int id in _perimeterModuleNumber)
                {
                    //cell.Propagate(direction, id);
                }
            }
        }
    }
}