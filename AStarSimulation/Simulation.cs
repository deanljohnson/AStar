using System;
using System.Collections.Generic;
using System.Diagnostics;
using AStar;
using AStarSimulation.Grids.Hexagon;
using AStarSimulation.Grids.Square;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFNetHex;

namespace AStarSimulation
{
    public class Simulation
    {
        private readonly Dictionary<CellState, Color> m_StateToColorMap = new Dictionary<CellState, Color>
        {
            {CellState.None, Color.Black},
            {CellState.Open, Color.Yellow},
            {CellState.Closed, Color.Blue},
            {CellState.End, Color.Red},
            {CellState.Start, Color.Green},
            {CellState.Path, Color.Cyan},
            {CellState.Wall, new Color(200, 200, 200)}
        }; 

        private readonly Stopwatch m_Stopwatch = new Stopwatch();
        private PathfindingData m_Data;

        private readonly RenderWindow m_Window;

        private IIndexedPathfindingMap m_Grid;
        private const double WALL_DENSITY = .95;
        private AStar<Vector2i> m_AStar;
        private Vector2i m_Start;
        private Vector2i m_End;
        private PathfindingGraphState m_GraphState;
        private SimulationAction m_SimulationAction;
        private GridType m_GridType;
        private Vector2i m_NodeSize;

        public Vector2i NodeSize {
            get { return m_NodeSize; }
            set
            {
                if (m_NodeSize == value)
                {
                    return;
                }

                m_NodeSize = value;
                RebuildGraph();
            }
        }

        public GridType GridType
        {
            get { return m_GridType; }
            set
            {
                if (m_GridType == value) return;

                m_GridType = value;
                RebuildGraph();
            }
        }

        public SimulationAction SimulationAction
        {
            get { return m_SimulationAction; }
            set {
                if (m_SimulationAction == value) return;

                SwitchSimulationAction(value);
            }
        }

        public Simulation(RenderWindow window, GridType gridType, Vector2i nodeSize)
        {
            m_Window = window;
            m_Window.MouseButtonPressed += MousePressedEvent;
            m_Window.MouseMoved += MouseMovedEvent;

            SimulationAction = SimulationAction.None;

            m_NodeSize = nodeSize;
            m_GridType = gridType;
            RebuildGraph();
        }

        public void Update()
        {
            switch (SimulationAction)
            {
                case SimulationAction.RunContinuously:
                    if (m_GraphState == PathfindingGraphState.Finished)
                    {
                        ResetGraph();
                    }
                    FullRunOnce();
                    break;
                case SimulationAction.RunOnce:
                    FullRunOnce();
                    SimulationAction = SimulationAction.None;
                    break;
                case SimulationAction.RunOneStep:
                    RunOneStep();
                    SimulationAction = SimulationAction.None;
                    break;
            }
        }

        public void Render()
        {
            m_Window.Draw(m_Grid);
        }

        private void RebuildGraph()
        {
            switch (m_GridType)
            {
                case GridType.Square:
                    m_Grid = BuildSquareGrid(m_NodeSize);
                    SimulationAction = SimulationAction.None;
                    ResetGraph();
                    break;
                case GridType.Hex:
                    m_Grid = BuildHexGrid(m_NodeSize);
                    SimulationAction = SimulationAction.None;
                    ResetGraph();
                    break;
            }
        }

        private void SwitchSimulationAction(SimulationAction newAction)
        {
            m_SimulationAction = SimulationAction.None;
            switch (newAction)
            {
                case SimulationAction.RunContinuously:
                    m_SimulationAction = newAction;
                    break;
                case SimulationAction.RunOnce:
                    if (m_GraphState == PathfindingGraphState.Finished)
                    {
                        ResetGraph();
                    }
                    else
                    {
                        m_SimulationAction = newAction;
                    }
                    break;
                case SimulationAction.RunOneStep:
                    if (m_GraphState == PathfindingGraphState.Finished)
                    {
                        ResetGraph();
                    }
                    else
                    {
                        m_SimulationAction = newAction;
                    }
                    break;
            }
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
                m_GraphState = PathfindingGraphState.InProgress;
                ColorGridFromPathData(null);
            }
        }

        private void ReportPathFinished(Stack<Vector2i> path)
        {
            m_GraphState = PathfindingGraphState.Finished;
            var pathFindingTime = m_Stopwatch.ElapsedMilliseconds;
            m_Stopwatch.Reset();

            var pathLength = path.Count;
            var nodesVisited = m_AStar.Open.Count + m_AStar.Closed.Count;

            ColorGridFromPathData(path);

            m_Data.HeuristicUsed = m_AStar.HeuristicScale;
            m_Data.GraphSize = m_Grid.Count;
            m_Data.PathfindingTime = pathFindingTime;
            m_Data.TotalPathfindingTime += pathFindingTime;
            m_Data.PathLength = pathLength;
            m_Data.NodesVisited = nodesVisited;
            m_Data.TotalNodesVisited += nodesVisited;
            m_Data.PathsComputed++;
            m_Data.OutputData();
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
            m_GraphState = PathfindingGraphState.Ready;
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

        private IIndexedPathfindingMap BuildSquareGrid(Vector2i nodeSize)
        {
            var gridSize = new Vector2i((int) (m_Window.Size.X/nodeSize.X), (int) (m_Window.Size.Y/nodeSize.Y));
            return new SquareGrid(nodeSize, gridSize, m_StateToColorMap);
        }

        private IIndexedPathfindingMap BuildHexGrid(Vector2i hexSize)
        {
            var floatHexSize = new Vector2f(hexSize.X, hexSize.Y);
            var testHex = new HexShape(new Layout(Orientation.Flat, floatHexSize, new Vector2f(0, 0)));
            var size = new Vector2f(testHex.GetLocalBounds().Width, testHex.GetLocalBounds().Height);
            //We subtract one to handle the center hex
            var vertRadius = m_Window.Size.Y/(2f*size.Y) - 1;
            var horizRadius = m_Window.Size.X/(2f*size.X) - 1;
             
            return new HexGrid((int) Math.Min(vertRadius, horizRadius), Orientation.Flat, floatHexSize, m_StateToColorMap)
            { Position = new Vector2f(m_Window.Size.X / 2f, m_Window.Size.Y / 2f) };
        }

        //Kept here for future reference for when I revisit the idea of automatic obstacles
        /*private void BuildObstacles()
        {
            for (var y = 0; y < m_Grid.Dimensions.Y; y++)
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
            }
        }*/

        private void MousePressedEvent(object sender, MouseButtonEventArgs e)
        {
            if (e.Button.Equals(Mouse.Button.Left))
            {
                try
                {
                    var nodeClicked = m_Grid.PixelToIndex(new Vector2i(e.X, e.Y));
                    m_Grid.Set(nodeClicked, CellState.Wall);
                }
                catch (ArgumentException)
                {
                    //The mouse was not on the grid, so we just ignore this mouse event
                }
            }
        }

        private void MouseMovedEvent(object sender, MouseMoveEventArgs e)
        {
            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                try
                {
                    var nodeAtMouse = m_Grid.PixelToIndex(new Vector2i(e.X, e.Y));
                    var toMakeWalls = m_Grid.NeighborsOfCell(nodeAtMouse);
                    toMakeWalls.Add(nodeAtMouse);

                    foreach (var i in toMakeWalls)
                    {
                        m_Grid.Set(i, CellState.Wall);
                    }
                }
                catch (ArgumentException)
                {
                    //The mouse was not on the grid, so we just ignore this mouse event
                }
            }
        }
    }
}