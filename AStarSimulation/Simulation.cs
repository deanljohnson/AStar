using System;
using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using AStar;

namespace AStarSimulation
{
    internal class Simulation
    {
        private readonly Stopwatch m_Stopwatch = new Stopwatch();
        private long m_TotalNodesVisited;
        private long m_TotalPathFindingTime;
        private long m_PathsComputed;

        private const Keyboard.Key START_CONTINUOUS_KEY = Keyboard.Key.Space;
        private const Keyboard.Key RUN_ONCE_KEY = Keyboard.Key.Return;

        private static readonly Random Random = new Random();
        private readonly RenderWindow m_Window;
        private readonly Grid m_Grid;
        private Vector2i m_Start;
        private Vector2i m_End;
        private bool m_RunContinuously;
        private bool m_RunThisUpdate;

        public Simulation(RenderWindow window)
        {
            var nodeSize = new Vector2i(10, 10);

            m_Grid = new Grid(nodeSize, new Vector2i((int)(window.Size.X / nodeSize.X), (int)(window.Size.Y / nodeSize.Y)), new Dictionary<CellState, Color>
            {
                {CellState.Open, Color.Yellow},
                {CellState.Closed, Color.Blue},
                {CellState.End, Color.Red},
                {CellState.Start, Color.Green},
                {CellState.Path, Color.Cyan},
                {CellState.Wall, new Color(200, 200, 200)}
            });

            m_Window = window;
            m_Window.KeyReleased += KeyReleasedEvent;
            m_Window.MouseButtonPressed += MousePressedEvent;
            m_Window.MouseMoved += MouseMovedEvent;

            AStar<Vector2i>.HeuristicScale = 1;
        }

        public void Update()
        {
            if (m_RunContinuously)
            {
                m_RunThisUpdate = true;
            }

            if (m_RunThisUpdate)
            {
                ResetGraph();
                RunOnce();
                m_RunThisUpdate = false;
            }
        }

        public void Render()
        {
            m_Window.Draw(m_Grid);
        }

        private void RunOnce()
        {
            m_Stopwatch.Start();
            var path = AStar<Vector2i>.PathFind(m_Start, m_End, GetNeighbors, DistanceBetweenNodes);
            m_Stopwatch.Stop();

            if (path == null)
            {
                throw new Exception("AStar returned a null path");
            }

            var pathFindingTime = m_Stopwatch.ElapsedMilliseconds;
            m_TotalPathFindingTime += pathFindingTime;
            m_PathsComputed++;
            m_Stopwatch.Reset();

            var pathLength = path.Count;
            var nodesVisited = AStar<Vector2i>.Open.Count() + AStar<Vector2i>.Closed.Count;
            m_TotalNodesVisited += nodesVisited;

            m_Grid.Set(AStar<Vector2i>.Open, CellState.Open);
            m_Grid.Set(AStar<Vector2i>.Closed, CellState.Closed);
            m_Grid.Set(path, CellState.Path);

            Console.Clear();
            Console.WriteLine("Heuristic: " + AStar<Vector2i>.HeuristicScale);
            Console.WriteLine("Graph Size: {0}({1} * {2})", (m_Grid.GridSize.X * m_Grid.GridSize.Y), m_Grid.GridSize.X, m_Grid.GridSize.Y);
            Console.WriteLine("Time Taken: " + pathFindingTime + "ms");
            Console.WriteLine("Path Length: " + pathLength);
            Console.WriteLine("Nodes Visited: " + nodesVisited);
            Console.WriteLine("Paths Computed: " + m_PathsComputed);
            Console.WriteLine("Average Nodes Visited: " + m_TotalNodesVisited / m_PathsComputed);
            Console.WriteLine("Average Time: " + m_TotalPathFindingTime / (float)m_PathsComputed + "ms");
            Console.WriteLine("Total Time in Pathfinding: " + m_TotalPathFindingTime / 1000f + "s");
        }

        private void ResetGraph()
        {
            ResetNodes();
            SetStartAndEnd();
            BuildObstacles();
        }

        private void ResetNodes()
        {
            m_Grid.SetAll(Color.Transparent);
        }

        private void SetStartAndEnd()
        {
            //m_Start = new Vector2i(0, 0);
            m_Start = new Vector2i(Random.Next(m_Grid.GridSize.X), Random.Next(m_Grid.GridSize.Y));
            m_Grid.Set(m_Start, CellState.Start);
            //m_End = new Vector2i(m_Grid.GridSize.X - 1, m_Grid.GridSize.Y - 1);
            m_End = new Vector2i(Random.Next(m_Grid.GridSize.X), Random.Next(m_Grid.GridSize.Y));
            m_Grid.Set(m_End, CellState.End);
        }

        private void BuildObstacles()
        {
            for (var y = 0; y < m_Grid.GridSize.Y; y++)
            {
                for (var x = 0; x < m_Grid.GridSize.X; x++)
                {
                    if ((y % 2) != 0 && (Random.Next(0, 10) < 7))
                    {
                        var i = new Vector2i(x, y);
                        if (i != m_Start && i != m_End)
                        {
                            m_Grid.Set(i, CellState.Wall);
                        }
                    }
                }
            }
        }

        private static double DistanceBetweenNodes(Vector2i a, Vector2i b)
        {
            return Utils.Distance(new Vector2f(a.X, a.Y), new Vector2f(b.X, b.Y));
        }

        private List<Vector2i> GetNeighbors(Vector2i current)
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
                if (m_Grid.Is(neighbors[i], CellState.Wall))
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
            return index.Y == m_Grid.GridSize.Y - 1;
        }

        private bool OnLeft(Vector2i index)
        {
            return index.X == 0;
        }

        private bool OnRight(Vector2i index)
        {
            return index.X == m_Grid.GridSize.X;
        }

        private void KeyReleasedEvent(object sender, KeyEventArgs e)
        {
            if (e.Code.Equals(START_CONTINUOUS_KEY))
            {
                m_RunContinuously = !m_RunContinuously;
            }
            else if (e.Code.Equals(RUN_ONCE_KEY))
            {
                m_RunThisUpdate = true;
            }
        }

        private void MousePressedEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(Mouse.Button.Left))
            {
                var nodeClicked = IndexOfPixel(new Vector2i(e.X, e.Y));

                m_Grid.Set(nodeClicked, CellState.Wall);
            }
        }

        private void MouseMovedEvent(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var nodeClicked = IndexOfPixel(new Vector2i(e.X, e.Y));

                m_Grid.Set(nodeClicked, CellState.Wall);
                var neighbors = GetNeighbors(nodeClicked);
                foreach (var i in neighbors)
                {
                    m_Grid.Set(i, CellState.Wall);
                }
            }
        }

        private Vector2i IndexOfPixel(Vector2i pos)
        {
            var index = new Vector2i(pos.X / m_Grid.CellSize.X, pos.Y / m_Grid.CellSize.Y);
            return index;
        }
    }
}