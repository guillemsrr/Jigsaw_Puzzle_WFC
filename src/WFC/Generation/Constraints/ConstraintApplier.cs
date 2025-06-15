using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Generation.Cells;

namespace GodotTest.WFC.Generation.Constraints
{
    public interface ConstraintApplier
    {
        void ApplyConstraint(Dictionary<Vector3I, CellController> cells);
    }
}