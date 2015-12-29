using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AStarSimulation.Grids.Square
{
    internal class SquareGrid : SquareGridBase, IIndexedPathfindingMap
    {
        private static readonly Random Random = new Random();

        private Dictionary<CellState, Color> m_StateToColorMap { get; }

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
            return index;
        }

        public double DistanceEstimate(Vector2i a, Vector2i b)
        {
            return Utils.Distance(new Vector2f(a.X, a.Y), new Vector2f(b.X, b.Y));
        }

        public List<Vector2i> NeighborsOfCell(Vector2i current)
        {
            var neighbors = new List<Vector2i>();

            var onTop = OnTop(current);
            var onBottom = OnBottom(current);
            var onLeft = OnLeft(current);
            var onRight = OnRight(current);

            if (!onTop)
            {
                neighbors.Add(new Vector2i(current.X, current.Y - 1));

                if (!onLeft)
                    neighbors.Add(new Vector2i(current.X - 1, current.Y - 1));
                if (!onRight)
                    neighbors.Add(new Vector2i(current.X + 1, current.Y - 1));
            }
            if (!onBottom)
            {
                neighbors.Add(new Vector2i(current.X, current.Y + 1));

                if (!onLeft)
                    neighbors.Add(new Vector2i(current.X - 1, current.Y + 1));
                if (!onRight)
                    neighbors.Add(new Vector2i(current.X + 1, current.Y + 1));
            }
            if (!onLeft)
                neighbors.Add(new Vector2i(current.X - 1, current.Y));
            if (!onRight)
                neighbors.Add(new Vector2i(current.X + 1, current.Y));

            for (var i = 0; i < neighbors.Count; i++)
            {
                if (Is(neighbors[i], CellState.Wall))
                {
                    neighbors.RemoveAt(i);
                    i--;
                }
            }

            return neighbors;
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
    }
}
