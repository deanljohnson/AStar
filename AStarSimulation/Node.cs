using SFML.Graphics;
using SFML.Window;

namespace AStarSimulation
{
    internal class Node : Transformable, Drawable
    {
        private readonly RectangleShape m_shape;

        public static Vector2i Size = new Vector2i(0, 0);

        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public bool IsOpen { get; set; }
        public bool IsClosed { get; set; }
        public bool IsOptimal { get; set; }
        public bool IsWall { get; set; }

        public Node(Vector2f pos)
        {
            Position = pos;
            m_shape = new RectangleShape(new Vector2f(Size.X, Size.Y))
            {
                OutlineColor = new Color(192, 192, 192),
                OutlineThickness = -1,
                FillColor = Color.Black,
                Position = Position
            };
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (IsOptimal)
                m_shape.FillColor = Color.Cyan;
            else if (IsWall)
            {
                m_shape.FillColor = Color.White;
                m_shape.OutlineColor = Color.White;
            }
            else if (IsEnd)
                m_shape.FillColor = Color.Red;
            else if (IsStart)
                m_shape.FillColor = Color.Green;
            else if (IsOpen)
                m_shape.FillColor = Color.Yellow;
            else if (IsClosed)
                m_shape.FillColor = Color.Blue;

            target.Draw(m_shape);
        }
    }
}