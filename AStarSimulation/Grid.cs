using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AStarSimulation
{
    internal class Grid : SquareGridBase
    {
        private Dictionary<CellState, Color> m_StateToColorMap { get; }

        public Grid(Vector2i cellSize, Vector2i gridSize, Dictionary<CellState, Color> stateToColorMap)
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

        public void SetAll(Color color)
        {
            ClearCellColors(color);
        }

        public bool Is(Vector2i i, CellState s)
        {
            return GetColorOfCell(i) == m_StateToColorMap[s];
        }
    }
}
