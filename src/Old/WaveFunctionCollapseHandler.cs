// WaveFunctionCollapseHandler.cs
// Implements a simple WFC/backtracking for puzzle pieces based on adjacency data.

using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotTest.WFC;
using GodotTest.WFC.Adjacency;

namespace GodotTest.Old
{
    public class WaveFunctionCollapseHandler
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
        public PackedScene[] PiecePrefabs { get; set; }

        private AdjacencyData[] _adjacencies;

        public List<int>[,] Wave;
        private Random _rand = new Random();

        private readonly Vector2I[] _dirs = new[]
        {
            new Vector2I(0, -1), // Top (-Z)
            new Vector2I(0, 1), // Down (+Z)
            new Vector2I(1, 0), // Right (+X)
            new Vector2I(-1, 0), // Left (-X)
        };

        public WaveFunctionCollapseHandler(int rows, int cols, PackedScene[] prefabs)
        {
            Rows = rows;
            Columns = cols;
            PiecePrefabs = prefabs;

            Wave = new List<int>[Columns, Rows];
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Wave[x, y] = new List<int>(Enumerable.Range(0, PiecePrefabs.Length));
                }
            }

            _adjacencies = new AdjacencyData[PiecePrefabs.Length];
            for (int i = 0; i < PiecePrefabs.Length; i++)
            {
                var piece = PiecePrefabs[i].Instantiate<Puzzle.PuzzlePiece>();
                _adjacencies[i] = piece.AdjacencyData;
                piece.QueueFree();
            }
        }

        public bool Solve() => CollapseCell(0);

        private bool CollapseCell(int cellIndex)
        {
            if (cellIndex >= Columns * Rows)
                return true;

            int x = cellIndex % Columns;
            int y = cellIndex / Columns;
            var options = Wave[x, y];

            foreach (int choice in options.OrderBy(_ => _rand.Next()))
            {
                var backup = CopyWave();
                Wave[x, y] = new List<int> {choice};

                if (Propagate(x, y) && CollapseCell(cellIndex + 1))
                {
                    return true;
                }

                Wave = backup;
            }

            return false;
        }

        private bool Propagate(int x0, int y0)
        {
            var stack = new Stack<Vector2I>();
            stack.Push(new Vector2I(x0, y0));

            while (stack.Count > 0)
            {
                var pos = stack.Pop();
                int x = pos.X, y = pos.Y;
                var cellOptions = Wave[x, y];
                if (cellOptions.Count == 0)
                {
                    return false;
                }

                foreach (var dir in _dirs)
                {
                    int nx = x + dir.X, ny = y + dir.Y;
                    if (nx < 0 || nx >= Columns || ny < 0 || ny >= Rows)
                    {
                        continue;
                    }

                    var neighborOptions = Wave[nx, ny];
                    var validNeighbor = new List<int>();

                    foreach (int nIdx in neighborOptions)
                    {
                        foreach (int cIdx in cellOptions)
                        {
                            if (CheckMatch(cIdx, nIdx, dir))
                            {
                                validNeighbor.Add(nIdx);
                                break;
                            }
                        }
                    }

                    if (validNeighbor.Count == 0)
                    {
                        return false;
                    }

                    if (validNeighbor.Count < neighborOptions.Count)
                    {
                        Wave[nx, ny] = validNeighbor;
                        stack.Push(new Vector2I(nx, ny));
                    }
                }
            }

            return true;
        }

        private bool CheckMatch(int cIdx, int nIdx, Vector2I dir)
        {
            var cRes = _adjacencies[cIdx];
            var nRes = _adjacencies[nIdx];

            var a = dir switch
            {
                var d when d == new Vector2I(0, -1) => cRes.Top,
                var d when d == new Vector2I(0, 1) => cRes.Down,
                var d when d == new Vector2I(1, 0) => cRes.Right,
                _ => cRes.Left,
            };
            var b = dir switch
            {
                var d when d == new Vector2I(0, -1) => nRes.Down,
                var d when d == new Vector2I(0, 1) => nRes.Top,
                var d when d == new Vector2I(1, 0) => nRes.Left,
                _ => nRes.Right,
            };

            return (a.Type == AdjacencyType.Flat && b.Type == AdjacencyType.Flat)
                   || (a.Type == AdjacencyType.Tab && b.Type == AdjacencyType.Slot)
                   || (a.Type == AdjacencyType.Slot && b.Type == AdjacencyType.Tab);
        }

        private List<int>[,] CopyWave()
        {
            var copy = new List<int>[Columns, Rows];
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    copy[x, y] = new List<int>(Wave[x, y]);
                }
            }

            return copy;
        }
    }
}