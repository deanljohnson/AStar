using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SFNetHex;

namespace PathViewSimulation.Grids.Hexagon
{
    internal class HexGrid : DrawableHexSet, IIndexedPathfindingMap
    {
        private static readonly Random Random = new Random();
        private Dictionary<CellState, Color> m_StateToColorMap { get; }

        public bool IsUniform => true;

        public int Count => ColorTable.Count;

        public HexGrid(int rad, Orientation o, Vector2f cellSize, Dictionary<CellState, Color> stateToColorMap) 
            : base(rad, o, cellSize)
        {
            m_StateToColorMap = stateToColorMap;
        }

        public void Set(Vector2i i, CellState s)
        {
            SetColorOfCell(i.X, i.Y, m_StateToColorMap[s]);
        }

        public void Set(IEnumerable<Vector2i> indices, CellState s)
        {
            foreach (var i in indices)
            {
                SetColorOfCell(i.X, i.Y, m_StateToColorMap[s]);
            }
        }

        public void SetAll(CellState s)
        {
            ClearCellColors(m_StateToColorMap[s]);
        }

        public bool Is(Vector2i i, CellState s)
        {
            return GetColorOfCell(i.X, i.Y) == m_StateToColorMap[s];
        }

        public Vector2i RandomOpenCell()
        {
            while (true)
            {
                var index = Random.Next(ColorTable.Count);
                var hex = ColorTable.ElementAt(index).Key;
                var v = new Vector2i(hex.X, hex.Y);

                if (!Is(v, CellState.Wall))
                    return v;
            }
            
        }

        public Vector2i PixelToIndex(Vector2i p)
        {
            var i = GetNearestWholeHex(new Vector2f(p.X, p.Y));

            if (ColorTable.ContainsKey(i))
            {
                return new Vector2i(i.X, i.Y);
            }
            throw new ArgumentException($"No index corresponds to {p}");
        }

        public double DistanceEstimate(Vector2i a, Vector2i b)
        {
            return Utils.Distance(new Vector2f(a.X, a.Y), new Vector2f(b.X, b.Y));
        }

        public List<Vector2i> GetNeighbors(Vector2i cell)
        {
            var neighbors = new List<Vector2i>
            {
                new Vector2i(cell.X - 1, cell.Y),
                new Vector2i(cell.X + 1, cell.Y),
                new Vector2i(cell.X, cell.Y - 1),
                new Vector2i(cell.X, cell.Y + 1),
                new Vector2i(cell.X - 1, cell.Y + 1),
                new Vector2i(cell.X + 1, cell.Y - 1),
            };

            for (var i = 0; i < neighbors.Count; i++)
            {
                if (!ColorTable.ContainsKey(new Hex(neighbors[i].X, neighbors[i].Y)) || Is(neighbors[i], CellState.Wall))
                {
                    neighbors.RemoveAt(i);
                    i--;
                }
            }

            return neighbors;
        }

        public List<Vector2i> GetJPSPrunedNeighbors(Vector2i current, Vector2i previous)
        {
            throw new NotImplementedException();
        }

        public List<Vector2i> CellsInLine(Vector2i a, Vector2i b)
        {
            var hexes = GetHexesInLine(new Hex(a.X, a.Y), new Hex(b.X, b.Y));

            return new List<Vector2i>(hexes.Select(h => new Vector2i(h.X, h.Y)));
        }
    }
}
