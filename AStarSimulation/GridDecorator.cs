using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AStarSimulation
{
    internal class GridDecorator
    {
        private Grid m_Grid { get; }
        private Dictionary<CellState, Color> m_StateToColorMap { get; }

        public GridDecorator(Grid grid, Dictionary<CellState, Color> stateToColorMap)
        {
            m_Grid = grid;
            m_StateToColorMap = stateToColorMap;
        }

        public void Set(Vector2i i, CellState s)
        {
            m_Grid.SetColorOfCell(i, m_StateToColorMap[s]);
        }

        public void Set(IEnumerable<Vector2i> indices, CellState s)
        {
            foreach (var i in indices)
            {
                m_Grid.SetColorOfCell(i, m_StateToColorMap[s]);
            }
        }

        public bool Is(Vector2i i, CellState s)
        {
            return m_Grid.GetColorOfCell(i) == m_StateToColorMap[s];
        }
    }
}
