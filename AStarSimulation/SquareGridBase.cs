using SFML.Graphics;
using SFML.System;

namespace AStarSimulation
{
    internal class SquareGridBase : Transformable, Drawable
    {
        private readonly VertexArray m_QuadArray;
        private readonly VertexArray m_LineArray;

        public Vector2i GridSize { get; }
        public Vector2i CellSize { get; }

        public SquareGridBase(Vector2i cellSize, Vector2i gridSize)
        {
            GridSize = gridSize;
            CellSize = cellSize;
            m_QuadArray = new VertexArray(PrimitiveType.Quads, (uint) (gridSize.X * gridSize.Y * 4));
            m_LineArray = new VertexArray(PrimitiveType.Lines, (uint) (gridSize.X + gridSize.Y) * 2);

            for (uint y = 0; y < gridSize.Y; y++)
            {
                for (uint x = 0; x < gridSize.X; x++)
                {
                    var i = (uint) ((x + (y * gridSize.X)) * 4);
                    var topLeft = new Vector2f(cellSize.X*x, cellSize.Y*y);
                    var topRight = topLeft + new Vector2f(cellSize.X, 0);
                    var bottomLeft = topLeft + new Vector2f(0, cellSize.Y);
                    var bottomRight = topLeft + new Vector2f(cellSize.X, cellSize.Y);

                    m_QuadArray[i] = new Vertex(topLeft, Color.Black);
                    m_QuadArray[i + 1] = new Vertex(topRight, Color.Black);
                    m_QuadArray[i + 2] = new Vertex(bottomRight, Color.Black);
                    m_QuadArray[i + 3] = new Vertex(bottomLeft, Color.Black);
                }
            }

            for (uint x = 0; x < gridSize.X; x++)
            {
                m_LineArray[x*2] = new Vertex(new Vector2f(x * cellSize.X, 0), Color.White);
                m_LineArray[(x*2) + 1] = new Vertex(new Vector2f(x * cellSize.X, cellSize.Y * gridSize.Y), Color.White);
            }

            var yOffset = (uint) (gridSize.X*2);
            for (uint y = 0; y < gridSize.Y; y++)
            {
                m_LineArray[yOffset + (y*2)] = new Vertex(new Vector2f(0, y * cellSize.Y), Color.White);
                m_LineArray[yOffset + (y*2) + 1] = new Vertex(new Vector2f(cellSize.X * gridSize.X, y * cellSize.Y), Color.White);
            }
        }

        protected void SetColorOfCell(int x, int y, Color color)
        {
            var i = (uint)((x + (y * GridSize.X)) * 4);

            m_QuadArray[i] = new Vertex(m_QuadArray[i].Position, color);
            m_QuadArray[i + 1] = new Vertex(m_QuadArray[i + 1].Position, color);
            m_QuadArray[i + 2] = new Vertex(m_QuadArray[i + 2].Position, color);
            m_QuadArray[i + 3] = new Vertex(m_QuadArray[i + 3].Position, color);
        }

        protected void SetColorOfCell(Vector2i i, Color color)
        {
            SetColorOfCell(i.X, i.Y, color);
        }

        protected Color GetColorOfCell(int x, int y)
        {
            var i = (uint)((x + (y * GridSize.X)) * 4);

            return m_QuadArray[i].Color;
        }

        protected Color GetColorOfCell(Vector2i i)
        {
            return GetColorOfCell(i.X, i.Y);
        }

        protected void ClearCellColors(Color color)
        {
            for (var y = 0; y < GridSize.Y; y++)
            {
                for (var x = 0; x < GridSize.X; x++)
                {
                    SetColorOfCell(x, y, color);
                }
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform.Combine(Transform);

            target.Draw(m_QuadArray, states);
            target.Draw(m_LineArray);
        }
    }
}
