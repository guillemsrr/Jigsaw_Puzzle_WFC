using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Generation.Cells;
using GodotTest.WFC.Generation.Backtracking;
using GodotTest.WFC.Generation.Waves;
using GodotTest.WFC.Modules;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Generation
{
    public class WaveFunctionCollapse
    {
        private readonly Wave _wave;
        private List<CellController> _uncollapsedCells;
        private EntropyHeap _entropyHeap;
        private BacktrackingHandler _backtrackingHandler;
        private int _numberTries;
        private const int MAX_OBSERVATION_TRIES = 1000;

        private int NumberUncollapsedCells => _uncollapsedCells.Count;

        public WaveFunctionCollapse(Wave wave)
        {
            _wave = wave;
            _uncollapsedCells = new List<CellController>(wave.Cells.Values);
            _entropyHeap = new EntropyHeap(wave.Cells.Values);
            _backtrackingHandler = new BacktrackingHandler(this);
        }

        public bool Observe()
        {
            _backtrackingHandler.AddState(_wave, _uncollapsedCells, _entropyHeap);

            while (NumberUncollapsedCells != 0)
            {
                /*_numberTries++;
                if (_numberTries > MAX_OBSERVATION_TRIES)
                {
                    GD.PrintErr("Couldn't find a solution");
                    return false;
                }*/

                CellController randomCell = _entropyHeap.GetCell();
                if (randomCell == null)
                {
                    _entropyHeap.AddLowestEntropyCell(_uncollapsedCells);
                    randomCell = _entropyHeap.GetCell();
                }

                if (randomCell == null)
                {
                    _backtrackingHandler.DiscardCurrentState();
                    continue;
                }

                if (!randomCell.CanCollapse)
                {
                    continue;
                }

                Collapse(randomCell);
                Propagate(randomCell);
            }

            return true;
        }

        private void Collapse(CellController randomCell)
        {
            if (!randomCell.CanCollapse)
            {
                return;
            }

            randomCell.Collapse();
            RemoveCollapsedCell(randomCell);
        }

        private void RemoveCollapsedCell(CellController randomCell)
        {
            _uncollapsedCells.Remove(randomCell);
        }

        private void Propagate(CellController collapsedCell)
        {
            Queue<CellController> cellsToUpdate = new();
            cellsToUpdate.Enqueue(collapsedCell);

            while (cellsToUpdate.Count != 0)
            {
                CellController cell = cellsToUpdate.Dequeue();
                if (!cell.IsCollapsed && cell.OnlyOnePossibility)
                {
                    Collapse(cell);
                }

                if (!cell.IsCollapsed)
                {
                    return;
                }

                foreach ((Direction direction, Vector3I offsetVec) in Directions.DirectionsByVectors)
                {
                    if (Directions.IsVertical(direction))
                    {
                        continue;
                    }

                    Vector3I neighborPos = cell.Position + offsetVec;
                    if (!_wave.Cells.TryGetValue(neighborPos, out CellController neighborCell))
                    {
                        continue;
                    }

                    if (neighborCell.IsCollapsed)
                    {
                        continue;
                    }

                    ModuleData collapsedModule = cell.CellData.CollapsedModuleData;

                    bool changed = neighborCell.Propagate(direction, collapsedModule);
                    if (neighborCell.CellData.IsErroneus)
                    {
                        _backtrackingHandler.DiscardCurrentState();
                        return;
                    }

                    if (changed)
                    {
                        cellsToUpdate.Enqueue(neighborCell);
                    }
                }
            }

            _backtrackingHandler.AddState(_wave, _uncollapsedCells, _entropyHeap);
        }

        public void SetState(TrackingState trackingState)
        {
            _wave.SetCellsData(trackingState.WaveData);
            _uncollapsedCells = new List<CellController>(trackingState.UncollapsedCells);
            _entropyHeap = new EntropyHeap(trackingState.EntropyHeap);
        }
    }
}