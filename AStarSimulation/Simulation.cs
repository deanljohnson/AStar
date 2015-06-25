using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace AStarSimulation
{
    internal class Simulation
    {
        private static readonly Random Random = new Random();
        private readonly RenderWindow m_window;
        private List<Node> m_nodes = new List<Node>();
        private Node m_start;
        private Node m_end;
        private Node m_end2;
        private bool m_runPathFinding;
        private bool m_simComplete;
        private int m_width;
        private int m_height;

        public Simulation(RenderWindow window)
        {
            Node.Size = new Vector2i(10, 10);
            m_window = window;
            m_window.KeyReleased += KeyReleasedEvent;
            m_window.MouseButtonPressed += MousePressedEvent;
            m_window.MouseMoved += MouseMovedEvent;

            CreateNodes();
            AStar.AStar<Node>.HeuristicScale = 1;
        }

        public void Update()
        {
            if (m_runPathFinding)
            {
                var path = AStar.AStar<Node>.PathFind(m_start, new List<Node>{m_end,m_end2}, GetNeighbors, DistanceBetweenNodes);

                if (path != null)
                {
                    m_runPathFinding = false;
                    m_simComplete = true;

                    foreach (var node in path)
                    {
                        node.IsOptimal = true;
                    }

                    foreach (var node in AStar.AStar<Node>.Open)
                    {
                        node.IsOpen = true;
                    }

                    foreach (var node in AStar.AStar<Node>.Closed)
                    {
                        node.IsClosed = true;
                    }
                }
            }
        }

        public void Render()
        {
            foreach (var i in m_nodes)
            {
                m_window.Draw(i);
            }
        }

        private void CreateNodes()
        {
            m_nodes = new List<Node>();

            int posX = 0, posY = 0;
            m_width = (int) (m_window.Size.X / Node.Size.X);
            m_height = (int) (m_window.Size.Y / Node.Size.Y);

            while (m_nodes.Count < m_width * m_height)
            {
                m_nodes.Add(new Node(new Vector2f(posX, posY)));

                posX += Node.Size.X;

                if (posX > m_window.Size.X - Node.Size.X + 1)
                {
                    posX = 0;
                    posY += Node.Size.Y;
                }
            }

            m_start = m_nodes[Random.Next(m_nodes.Count)];
            m_start.IsStart = true;
            m_end = m_nodes[Random.Next(m_nodes.Count)];
            m_end.IsEnd = true;
            m_end2 = m_nodes[Random.Next(m_nodes.Count)];
            m_end2.IsEnd = true;
        }

        private static double DistanceBetweenNodes(Node a, Node b)
        {
            return Utils.Distance(a.Position, b.Position);
        }

        private List<Node> GetNeighbors(Node current)
        {
            var neighbors = new List<Node>();

            var index = m_nodes.IndexOf(current);
            if (index == -1)
                return null;

            var onTop = OnTop(index);
            var onBottom = OnBottom(index);
            var onLeft = OnLeft(index);
            var onRight = OnRight(index);

            if (!onTop)
            {
                neighbors.Add(m_nodes[index - m_width]);

                if (!onLeft)
                    neighbors.Add(m_nodes[index - m_width - 1]);
                if (!onRight)
                    neighbors.Add(m_nodes[index - m_width + 1]);
            }
            if (!onBottom)
            {
                neighbors.Add(m_nodes[index + m_width]);

                if (!onLeft)
                    neighbors.Add(m_nodes[index + m_width - 1]);
                if (!onRight)
                    neighbors.Add(m_nodes[index + m_width + 1]);
            }
            if (!onLeft)
                neighbors.Add(m_nodes[index - 1]);
            if (!onRight)
                neighbors.Add(m_nodes[index + 1]);

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (neighbors[i].IsWall)
                {
                    neighbors.RemoveAt(i);
                    i--;
                }
            }

            return neighbors;
        }

        private bool OnTop(int index)
        {
            return index < m_width;
        }

        private bool OnBottom(int index)
        {
            return index > (m_width * m_height) - m_width - 1;
        }

        private bool OnLeft(int index)
        {
            return index % m_width == 0;
        }

        private bool OnRight(int index)
        {
            return (index + 1) % m_width == 0;
        }

        private void KeyReleasedEvent(object sender, KeyEventArgs e)
        {
            if (e.Code.Equals(Keyboard.Key.Space))
            {
                if (m_simComplete)
                {
                    m_simComplete = false;
                    CreateNodes();
                }
                else if (!m_runPathFinding)
                    m_runPathFinding = !m_runPathFinding;
            }
        }

        private void MousePressedEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(Mouse.Button.Left))
            {
                Node nodeClicked = NodeContainingPixel(new Vector2i(e.X, e.Y));

                if (nodeClicked != null)
                    nodeClicked.IsWall = true;
            }
        }

        private void MouseMovedEvent(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var nodeClicked = NodeContainingPixel(new Vector2i(e.X, e.Y));

                if (nodeClicked != null)
                {
                    nodeClicked.IsWall = true;
                    var neighbors = GetNeighbors(nodeClicked);
                    foreach (var i in neighbors)
                    {
                        i.IsWall = true;
                    }
                }
            }
        }

        private Node NodeContainingPixel(Vector2i pos)
        {
            var moddedPos = new Vector2i(pos.X - (pos.X % Node.Size.X), pos.Y - (pos.Y % Node.Size.Y));

            foreach (var i in m_nodes)
            {
                var iPos = new Vector2i((int) i.Position.X, (int) i.Position.Y);

                if (moddedPos.X == iPos.X && moddedPos.Y == iPos.Y)
                    return i;
            }

            return null;
        }
    }
}