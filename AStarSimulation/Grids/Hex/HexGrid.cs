using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFNetHex;

namespace AStarSimulation.Grids.Hex
{
    internal class HexGrid : HexMap, IIndexedPathfindingMap
    {
        private static readonly Random Random = new Random();
        private Dictionary<CellState, Color> m_StateToColorMap { get; }

        public int Count => HexTable.Count;
        public Vector2i Dimensions => new Vector2i(0, 0);

        public HexGrid(int rad, Orientation o, Vector2f cellSize, Dictionary<CellState, Color> stateToColorMap) 
            : base(rad, o, cellSize)
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
                var allVectors = HexTable.Keys.ToList();
                var randomVector = allVectors[Random.Next(allVectors.Count)];

                if (!Is(randomVector, CellState.Wall))
                    return randomVector;
            }
            
        }

        public Vector2i PixelToIndex(Vector2i p)
        {
            var hex = HexUtils.PixelToHex(new Vector2f(p.X, p.Y), Layout);
            hex = hex.RoundHex();

            foreach (var hexColorPair in HexTable)
            {
                if (hexColorPair.Value.Hex == hex)
                {
                    return hexColorPair.Key;
                }
            }

            throw new ArgumentException($"No index corresponds to {p}");
        }

        public double DistanceEstimate(Vector2i a, Vector2i b)
        {
            return Utils.Distance(new Vector2f(a.X, a.Y), new Vector2f(b.X, b.Y));
        }

        public List<Vector2i> NeighborsOfCell(Vector2i current)
        {
            var neighbors = new List<Vector2i>
            {
                new Vector2i(current.X - 1, current.Y),
                new Vector2i(current.X + 1, current.Y),
                new Vector2i(current.X, current.Y - 1),
                new Vector2i(current.X, current.Y + 1),
                new Vector2i(current.X - 1, current.Y + 1),
                new Vector2i(current.X + 1, current.Y - 1),
            };

            for (var i = 0; i < neighbors.Count; i++)
            {
                try
                {
                    if (!HexTable.ContainsKey(neighbors[i]) || Is(neighbors[i], CellState.Wall))
                    {
                        neighbors.RemoveAt(i);
                        i--;
                    }
                }
                catch (ArgumentException)
                {
                }
            }

            return neighbors;
        }
    }
}
