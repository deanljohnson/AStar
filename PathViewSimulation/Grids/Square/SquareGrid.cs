using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace PathViewSimulation.Grids.Square
{
    internal class SquareGrid : SquareGridBase, IIndexedPathfindingMap
    {
        private static readonly Random Random = new Random();

        private Dictionary<CellState, Color> m_StateToColorMap { get; }

        public bool IsUniform => UseManhattanMetric;

        public bool UseManhattanMetric = false;

        public int Count => GridSize.X*GridSize.Y;

        public SquareGrid(Vector2i cellSize, Vector2i gridSize, Dictionary<CellState, Color> stateToColorMap)
            : base(cellSize, gridSize)
        {
            m_StateToColorMap = stateToColorMap;
        }

        public void Set(Vector2i i, CellState s)
        {
            SetColorOfCell(i, m_StateToColorMap[s]);
        }

        public void Set(IEnumerable<Vector2i> indices, CellState s)
        {
            foreach (var i in indices)
            {
                SetColorOfCell(i, m_StateToColorMap[s]);
            }
        }

        public void SetAll(CellState s)
        {
            ClearCellColors(m_StateToColorMap[s]);
        }

        public bool Is(Vector2i i, CellState s)
        {
            return GetColorOfCell(i) == m_StateToColorMap[s];
        }

        public Vector2i RandomOpenCell()
        {
            while (true)
            {
                var i = new Vector2i(Random.Next(GridSize.X), Random.Next(GridSize.Y));
                if (!Is(i, CellState.Wall))
                {
                    return i;
                }
            }
        }

        public Vector2i PixelToIndex(Vector2i p)
        {
            var index = new Vector2i(p.X / CellSize.X, p.Y / CellSize.Y);

            if (index.X < 0 || index.X >= GridSize.X || index.Y < 0 || index.Y >= GridSize.Y)
            {
                throw new ArgumentException($"No index corresponds to {p}");
            }

            return index;
        }

        public double DistanceEstimate(Vector2i a, Vector2i b)
        {
            if (UseManhattanMetric)
                return Math.Abs(b.X - a.X) + Math.Abs(b.Y - a.Y);

            return Utils.Distance(new Vector2f(a.X, a.Y), new Vector2f(b.X, b.Y));
        }

        public List<Vector2i> GetNeighbors(Vector2i cell)
        {
            var neighbors = new List<Vector2i>
            {
                new Vector2i(cell.X, cell.Y - 1),
                new Vector2i(cell.X, cell.Y + 1),
                new Vector2i(cell.X - 1, cell.Y),
                new Vector2i(cell.X + 1, cell.Y)
            };


            if (!UseManhattanMetric)
            {
                neighbors.Add(new Vector2i(cell.X - 1, cell.Y - 1));
                neighbors.Add(new Vector2i(cell.X + 1, cell.Y - 1));
                neighbors.Add(new Vector2i(cell.X - 1, cell.Y + 1));
                neighbors.Add(new Vector2i(cell.X + 1, cell.Y + 1));
            }

            CullOffGridAndWallCells(neighbors);

            return neighbors;
        }

        public List<Vector2i> GetJPSPrunedNeighbors(Vector2i current, Vector2i previous)
        {
            //TODO: Figure out how this can work with the Euclidean metric
            if (!UseManhattanMetric)
            {
                throw new InvalidOperationException("Cannot directionally prune neighbors without using the Manhattan metric");
            }

            if (current == previous)
            {
                return GetNeighbors(current);
            }

            var neighbors = new List<Vector2i>();

            //Vertical movement neighbors
            if (current.X == previous.X)
            {
                if (previous.Y > current.Y)
                {
                    neighbors.Add(new Vector2i(current.X, current.Y - 1));
                    
                    //Forced-Neighbors
                    if (Is(new Vector2i(current.X - 1, current.Y), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X - 1, current.Y - 1));
                    }
                    if (Is(new Vector2i(current.X + 1, current.Y), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X + 1, current.Y - 1));
                    }
                }
                if (previous.Y < current.Y)
                {
                    neighbors.Add(new Vector2i(current.X, current.Y + 1));

                    //Forced-Neighbors
                    if (Is(new Vector2i(current.X - 1, current.Y), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X - 1, current.Y + 1));
                    }
                    if (Is(new Vector2i(current.X + 1, current.Y), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X + 1, current.Y + 1));
                    }
                }
            }
            //Horizontal movement neighbors
            if (current.Y == previous.Y)
            {
                if (previous.X > current.X)
                {
                    neighbors.Add(new Vector2i(current.X - 1, current.Y));

                    //Forced-Neighbors
                    if (Is(new Vector2i(current.X, current.Y - 1), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X - 1, current.Y - 1));
                    }
                    if (Is(new Vector2i(current.X, current.Y + 1), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X - 1, current.Y + 1));
                    }
                }
                if (previous.X < current.X)
                {
                    neighbors.Add(new Vector2i(current.X + 1, current.Y));

                    //Forced-Neighbors
                    if (Is(new Vector2i(current.X, current.Y - 1), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X + 1, current.Y - 1));
                    }
                    if (Is(new Vector2i(current.X, current.Y + 1), CellState.Wall))
                    {
                        neighbors.Add(new Vector2i(current.X + 1, current.Y + 1));
                    }
                }
            }
            //Bottom-Right movement Neighbors
            if (current.X > previous.X && current.Y > previous.Y)
            {
                neighbors.Add(new Vector2i(current.X + 1, current.Y + 1));
                neighbors.Add(new Vector2i(current.X + 1, current.Y));
                neighbors.Add(new Vector2i(current.X, current.Y + 1));

                //Forced-Neighbors
                if (Is(new Vector2i(current.X, current.Y - 1), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X + 1, current.Y - 1));
                }
                if (Is(new Vector2i(current.X - 1, current.Y), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X - 1, current.Y + 1));
                }
            }
            //Top-Right movement Neighbors
            if (current.X > previous.X && current.Y < previous.Y)
            {
                neighbors.Add(new Vector2i(current.X + 1, current.Y - 1));
                neighbors.Add(new Vector2i(current.X + 1, current.Y));
                neighbors.Add(new Vector2i(current.X, current.Y - 1));

                //Forced-Neighbors
                if (Is(new Vector2i(current.X, current.Y + 1), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X + 1, current.Y + 1));
                }
                if (Is(new Vector2i(current.X - 1, current.Y), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X - 1, current.Y - 1));
                }
            }
            //Top-Left movement Neighbors
            if (current.X < previous.X && current.Y < previous.Y)
            {
                neighbors.Add(new Vector2i(current.X - 1, current.Y - 1));
                neighbors.Add(new Vector2i(current.X - 1, current.Y));
                neighbors.Add(new Vector2i(current.X, current.Y - 1));

                //Forced-Neighbors
                if (Is(new Vector2i(current.X, current.Y + 1), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X - 1, current.Y + 1));
                }
                if (Is(new Vector2i(current.X + 1, current.Y), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X + 1, current.Y - 1));
                }
            }
            //Bottom-Left movement Neighbors
            if (current.X < previous.X && current.Y > previous.Y)
            {
                neighbors.Add(new Vector2i(current.X - 1, current.Y + 1));
                neighbors.Add(new Vector2i(current.X - 1, current.Y));
                neighbors.Add(new Vector2i(current.X, current.Y + 1));

                //Forced-Neighbors
                if (Is(new Vector2i(current.X, current.Y - 1), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X - 1, current.Y - 1));
                }
                if (Is(new Vector2i(current.X + 1, current.Y), CellState.Wall))
                {
                    neighbors.Add(new Vector2i(current.X + 1, current.Y + 1));
                }
            }

            CullOffGridAndWallCells(neighbors);

            return neighbors;
        }

        public List<Vector2i> CellsInLine(Vector2i a, Vector2i b)
        {
            var dif = b - a;
            var difNormalized = Utils.Normalize(new Vector2f(dif.X, dif.Y));
            var length = Utils.Length(new Vector2f(dif.X, dif.Y));
            var interPointsCount = (int) length + 1;
            var cells = new HashSet<Vector2i> {a};

            var floatA = new Vector2f(a.X, a.Y);
            for (var i = 1; i <= interPointsCount; i++)
            {
                var floatPoint = floatA + (difNormalized * i);
                var point = new Vector2i((int) Math.Round(floatPoint.X), (int) Math.Round(floatPoint.Y));

                if (point.X > 0 && point.X < GridSize.X && point.Y > 0 && point.Y < GridSize.Y)
                {
                    cells.Add(point);
                }
            }

            cells.Add(b);
            return new List<Vector2i>(cells);
        }

        private bool OnTop(Vector2i index)
        {
            return index.Y == 0;
        }

        private bool OnBottom(Vector2i index)
        {
            return index.Y == GridSize.Y - 1;
        }

        private bool OnLeft(Vector2i index)
        {
            return index.X == 0;
        }

        private bool OnRight(Vector2i index)
        {
            return index.X == GridSize.X - 1;
        }

        private void CullOffGridAndWallCells(List<Vector2i> cells)
        {
            for (var i = 0; i < cells.Count; i++)
            {
                //Remove anything off the grid
                if (cells[i].X < 0 || cells[i].X >= GridSize.X
                    || cells[i].Y < 0 || cells[i].Y >= GridSize.Y
                    || Is(cells[i], CellState.Wall))
                {
                    cells.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
