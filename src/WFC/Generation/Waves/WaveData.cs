using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Generation.Cells;

namespace GodotTest.WFC.Generation.Waves
{
    public class WaveData
    {
        public Dictionary<Vector3I, CellData> CellDatas = new Dictionary<Vector3I, CellData>();

        public WaveData(Wave wave)
        {
            foreach (CellController cellController in wave.Cells.Values)
            {
                CellDatas[cellController.Position] = new CellData(cellController.CellData);
            }
        }
    }
}