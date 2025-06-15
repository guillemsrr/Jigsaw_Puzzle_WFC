using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Generation.Cells;

namespace GodotTest.WFC.Generation.Waves
{
    public class Wave
    {
        public Dictionary<Vector3I, CellController> Cells = new Dictionary<Vector3I, CellController>();

        public void SetCellsData(WaveData trackingStateWaveData)
        {
            foreach (CellController cellController in Cells.Values)
            {
                cellController.CellData = new CellData(trackingStateWaveData.CellDatas[cellController.Position]);
            }
        }
    }
}