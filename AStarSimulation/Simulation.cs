using System;
using System.Collections.Generic;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using AStar;
using AStarSimulation.Grids;
using AStarSimulation.Grids.Square;
using SFNetHex;

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
        private const Keyboard.Key RUN_ONE_STEP_KEY = Keyboard.Key.Right;
        private const double WALL_DENSITY = .95;

        private readonly RenderWindow m_Window;
        private AStar<Vector2i> m_AStar;
        private IIndexedPathfindingMap m_Grid;
        private Vector2i m_Start;
        private Vector2i m_End;
        private bool m_RunContinuously;
        private bool m_RunThisUpdate;
        private bool m_RunOneStep;
        private bool m_PathfindingComplete;

        public Simulation(RenderWindow window)
        {
            m_Window = window;
            m_Window.KeyReleased += KeyReleasedEvent;
            m_Window.MouseButtonPressed += MousePressedEvent;
            m_Window.MouseMoved += MouseMovedEvent;

            BuildSquareGrid(new Vector2i(30, 30));
            //BuildHexGrid(80, new Vector2f(3, 3));
            ResetGraph();
        }

        public void Update()
        {
            if (m_RunContinuously)
            {
                m_RunThisUpdate = true;
                if (m_PathfindingComplete)
                {
                    ResetGraph();
                }
            }

            if (m_RunThisUpdate)
            {
                FullRunOnce();
                m_RunThisUpdate = false;
            }
            else if (m_RunOneStep)
            {
                RunOneStep();
                m_RunOneStep = false;
            }
        }

        public void Render()
        {
            m_Window.Draw(m_Grid);
        }

        private void FullRunOnce()
        {
            m_Stopwatch.Start();
            var path = m_AStar.PathFind(m_Start, m_End);
            m_Stopwatch.Stop();

            if (path == null)
            {
                throw new Exception("AStar returned a null path");
            }

            ReportPathFinished(path);
        }

        private void RunOneStep()
        {
            m_Stopwatch.Start();
            var path = m_AStar.PathFindOneStep(m_Start, m_End);
            m_Stopwatch.Stop();

            if (path != null)
            {
                ReportPathFinished(path);
            }
            else
            {
                ColorGridFromPathData(null);
            }
        }

        private void ReportPathFinished(Stack<Vector2i> path)
        {
            m_PathfindingComplete = true;
            var pathFindingTime = m_Stopwatch.ElapsedMilliseconds;
            m_TotalPathFindingTime += pathFindingTime;
            m_PathsComputed++;
            m_Stopwatch.Reset();

            var pathLength = path.Count;
            var nodesVisited = m_AStar.Open.Count + m_AStar.Closed.Count;
            m_TotalNodesVisited += nodesVisited;

            ColorGridFromPathData(path);

            Console.Clear();
            Console.WriteLine("Heuristic: " + m_AStar.HeuristicScale);
            Console.WriteLine("Graph Size: {0}", m_Grid.Count);
            Console.WriteLine("Time Taken: " + pathFindingTime + "ms");
            Console.WriteLine("Path Length: " + pathLength);
            Console.WriteLine("Nodes Visited: " + nodesVisited);
            Console.WriteLine("Paths Computed: " + m_PathsComputed);
            Console.WriteLine("Average Nodes Visited: " + m_TotalNodesVisited / m_PathsComputed);
            Console.WriteLine("Average Time: " + m_TotalPathFindingTime / (float)m_PathsComputed + "ms");
            Console.WriteLine("Total Time in Pathfinding: " + m_TotalPathFindingTime / 1000f + "s");
        }

        private void ColorGridFromPathData(Stack<Vector2i> path)
        {
            m_Grid.Set(m_AStar.Open, CellState.Open);
            m_Grid.Set(m_AStar.Closed, CellState.Closed);

            //We have to run this check because step-by-step solving might pass a null path
            if (path != null)
            {
                m_Grid.Set(path, CellState.Path);
            }
            
            m_Grid.Set(m_Start, CellState.Start);
            m_Grid.Set(m_End, CellState.End);
        }

        private void ResetGraph()
        {
            m_AStar = new AStar<Vector2i>(m_Grid.NeighborsOfCell, m_Grid.DistanceEstimate);
            ResetNodes();
            SetStartAndEnd();
            m_PathfindingComplete = false;
            //BuildObstacles();
        }

        private void ResetNodes()
        {
            m_Grid.SetAll(CellState.None);
        }

        private void SetStartAndEnd()
        {
            do
            {
                m_Start = m_Grid.RandomOpenCell();
                m_End = m_Grid.RandomOpenCell();
            } while (m_Start == m_End);


            m_Grid.Set(m_Start, CellState.Start);
            m_Grid.Set(m_End, CellState.End);
        }

        private void BuildSquareGrid(Vector2i nodeSize)
        {
            m_Grid = new SquareGrid(nodeSize, new Vector2i((int)(m_Window.Size.X / nodeSize.X), (int)(m_Window.Size.Y / nodeSize.Y)), new Dictionary<CellState, Color>
            {
                {CellState.None, Color.Black},
                {CellState.Open, Color.Yellow},
                {CellState.Closed, Color.Blue},
                {CellState.End, Color.Red},
                {CellState.Start, Color.Green},
                {CellState.Path, Color.Cyan},
                {CellState.Wall, new Color(200, 200, 200)}
            });
        }

        private void BuildHexGrid(int radius, Vector2f hexSize)
        {
            m_Grid = new HexGrid(radius, Orientation.Flat, hexSize, new Dictionary<CellState, Color>
            {
                {CellState.None, Color.Black},
                {CellState.Open, Color.Yellow},
                {CellState.Closed, Color.Blue},
                {CellState.End, Color.Red},
                {CellState.Start, Color.Green},
                {CellState.Path, Color.Cyan},
                {CellState.Wall, new Color(200, 200, 200)}
            })
            { Position = new Vector2f(m_Window.Size.X / 2f, m_Window.Size.Y / 2f) };
        }

        //Kept here for future reference for when I revisit the idea of automatic obstacles
        private void BuildObstacles()
        {
            /*for (var y = 0; y < m_Grid.Dimensions.Y; y++)
            {
                for (var x = 0; x < m_Grid.Dimensions.X; x++)
                {
                    if ((y % 2) != 0 && (Random.NextDouble() < WALL_DENSITY))
                    {
                        var i = new Vector2i(x, y);
                        if (i != m_Start && i != m_End)
                        {
                            m_Grid.Set(i, CellState.Wall);
                        }
                    }
                }
            }*/
        }

        private void KeyReleasedEvent(object sender, KeyEventArgs e)
        {
            if (e.Code.Equals(START_CONTINUOUS_KEY))
            {
                m_RunContinuously = !m_RunContinuously;
            }
            else if (e.Code.Equals(RUN_ONCE_KEY))
            {
                if (m_PathfindingComplete)
                {
                    ResetGraph();
                }
                else
                {
                    m_RunThisUpdate = true;
                }
                
            }
            else if (e.Code.Equals(RUN_ONE_STEP_KEY))
            {
                if (m_PathfindingComplete)
                {
                    ResetGraph();
                }
                else
                {
                    m_RunOneStep = true;
                }
            }
        }

        private void MousePressedEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(Mouse.Button.Left))
            {
                var nodeClicked = m_Grid.PixelToIndex(new Vector2i(e.X, e.Y));

                m_Grid.Set(nodeClicked, CellState.Wall);
            }
        }

        private void MouseMovedEvent(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                var nodeClicked = m_Grid.PixelToIndex(new Vector2i(e.X, e.Y));

                m_Grid.Set(nodeClicked, CellState.Wall);
                var neighbors = m_Grid.NeighborsOfCell(nodeClicked);
                foreach (var i in neighbors)
                {
                    m_Grid.Set(i, CellState.Wall);
                }
            }
        }
    }
}