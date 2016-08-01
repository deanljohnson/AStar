using System;
using System.Collections.Generic;
using System.Diagnostics;
using PathView;
using PathViewSimulation.Grids.Hexagon;
using PathViewSimulation.Grids.Square;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SFNetHex;

namespace PathViewSimulation
{
    public class Simulation
    {
        private static readonly Dictionary<CellState, Color> StateToColorMap = new Dictionary<CellState, Color>
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

        private readonly RenderWindow m_Window;

        private readonly SolverFactory m_SolverFactory = new SolverFactory();
        private IIndexedPathfindingMap m_Grid;
        private IPathFinder<Vector2i> m_PathFinder;
        private IPathFindingListener<Vector2i> m_PathFindingListener; 
        private Vector2i m_Start;
        private Vector2i m_End;
        private PathfindingGraphState m_GraphState;
        private SimulationAction m_SimulationAction;
        private AlgorithmType m_AlgorithmType;
        private GridType m_GridType;
        private Vector2i m_NodeSize;
        private double m_Heuristic;
        private Stopwatch m_Timer;

        /// <summary>
        /// The size of the nodes currently in use by the Simulation
        /// </summary>
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

        /// <summary>
        /// The GridType currently in use by the Simulation
        /// </summary>
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

        public AlgorithmType AlgorithmType
        {
            get { return m_AlgorithmType; }
            set
            {
                if (m_AlgorithmType == value) return;

                m_AlgorithmType = value;
                RebuildGraph();
            }
        }

        /// <summary>
        /// The SimulationAction to run at the next call to Update
        /// </summary>
        public SimulationAction SimulationAction
        {
            get { return m_SimulationAction; }
            set {
                if (m_SimulationAction == value) return;

                SwitchSimulationAction(value);
            }
        }

        /// <summary>
        /// A scaling factor to be applied to distance 
        /// estimates during the pathfinding process.
        /// </summary>
        public double Heuristic {
            get { return m_Heuristic; }
            set
            {
                m_Heuristic = value;
                ResetGraph();
            }
        }

        public int MovesPerSecond { get; set; }
        public bool SaveEndPoints { get; set; }
        public bool GenerateWalls { get; set; }
        public PathfindingData Data { get; set; } = new PathfindingData();

        public Simulation(RenderWindow window, AlgorithmType algoType, GridType gridType, Vector2i nodeSize)
        {
            m_Window = window;
            m_Window.MouseButtonPressed += MousePressedEvent;
            m_Window.MouseMoved += MouseMovedEvent;

            SimulationAction = SimulationAction.None;

            m_NodeSize = nodeSize;
            m_AlgorithmType = algoType;
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
                case SimulationAction.RunProgressive:
                    if (m_GraphState == PathfindingGraphState.Finished)
                        SimulationAction = SimulationAction.None;
                    else if (m_Timer.ElapsedMilliseconds > (1000f/MovesPerSecond))
                    {
                        m_Timer.Restart();
                        RunOneStep();
                        if (m_GraphState == PathfindingGraphState.Finished)
                            SimulationAction = SimulationAction.None;
                    }
                    break;
            }
        }

        public void Render()
        {
            m_Window.Draw(m_Grid);
        }

        /// <summary>
        /// Rebuilds the graph based on the current GridType
        /// </summary>
        private void RebuildGraph()
        {
            m_Grid = IndexedPathfindingMapFactory.BuildMap(m_GridType, m_NodeSize, m_Window.Size, StateToColorMap);

            SimulationAction = SimulationAction.None;
            ResetGraph();
        }

        /// <summary>
        /// Switches to the given SimulationAction, or resets the graph 
        /// if it is in a state that cannot run the specified SimulationAction
        /// </summary>
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
                case SimulationAction.RunProgressive:
                    if (m_GraphState == PathfindingGraphState.Finished)
                    {
                        ResetGraph();
                    }
                    else
                    {
                        m_SimulationAction = newAction;
                        m_Timer = new Stopwatch();
                        m_Timer.Start();
                    }
                    
                    break;
            }
        }

        /// <summary>
        /// Finds a path from start to end on the graph
        /// </summary>
        private void FullRunOnce()
        {
            m_Stopwatch.Start();
            var path = m_PathFinder.PathFind(m_Start, m_End);
            m_Stopwatch.Stop();

            if (path == null)
            {
                throw new Exception("AStar returned a null path");
            }

            ReportPathFinished(path);
        }

        /// <summary>
        /// Runs a single step of the pathfinding process
        /// </summary>
        private void RunOneStep()
        {
            m_Stopwatch.Start();
            var path = m_PathFinder.PathFindOneStep(m_Start, m_End);
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

        /// <summary>
        /// Performs any necessary cleanup at the end of pathfinding and 
        /// populates the member PathFindingData instance with relevant data
        /// </summary>
        private void ReportPathFinished(Stack<Vector2i> path)
        {
            m_GraphState = PathfindingGraphState.Finished;
            var pathFindingTime = m_Stopwatch.Elapsed;
            m_Stopwatch.Reset();

            var pathLength = path.Count;
            var nodesVisited = m_PathFindingListener.Open.Count + m_PathFindingListener.Closed.Count;

            ColorGridFromPathData(path);

            Data.GraphSize = m_Grid.Count;
            Data.PathfindingTime = pathFindingTime;
            Data.TotalPathfindingTime += pathFindingTime;
            Data.PathLength = pathLength;
            Data.NodesVisited = nodesVisited;
            Data.TotalNodesVisited += nodesVisited;
            Data.TotalPathsComputed++;
        }

        /// <summary>
        /// Sets the colors on the grid to correspond with their CellState
        /// </summary>
        private void ColorGridFromPathData(Stack<Vector2i> path)
        {
            m_Grid.Set(m_PathFindingListener.Open, CellState.Open);
            m_Grid.Set(m_PathFindingListener.Closed, CellState.Closed);

            //We have to run this check because step-by-step solving might pass a null path
            if (path != null)
            {
                m_Grid.Set(path, CellState.Path);
            }
            
            m_Grid.Set(m_Start, CellState.Start);
            m_Grid.Set(m_End, CellState.End);
        }

        /// <summary>
        /// Resets the graph and the AStar instance so that pathfinding can be run again
        /// </summary>
        private void ResetGraph()
        {
            m_SolverFactory.CreateSolver(m_AlgorithmType, m_Grid, Heuristic, out m_PathFinder, out m_PathFindingListener);
            
            ResetNodes();

            if (GenerateWalls)
            {
                GenerateRandomWalls();
            }

            if (!SaveEndPoints)
            {
                SetStartAndEnd();
            }
            else
            {
                //The call to ResetNodes set everything the CellState.None, so we must reset these
                m_Grid.Set(m_Start, CellState.Start);
                m_Grid.Set(m_End, CellState.End);
            }
            m_GraphState = PathfindingGraphState.Ready;
        }

        /// <summary>
        /// Resets all nodes in the graph to CellState.None
        /// </summary>
        private void ResetNodes()
        {
            m_Grid.SetAll(CellState.None);
        }

        /// <summary>
        /// Generates random walls on the grid
        /// </summary>
        private void GenerateRandomWalls(int count = 5)
        {
            IWallGenerator gen = new StraightWallGenerator();
            gen.GenerateWalls(m_Grid, count);
        }

        /// <summary>
        /// Sets the start and end node on the Graph, guarenteeing they are not the same node.
        /// </summary>
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
                    var toMakeWalls = m_Grid.GetNeighbors(nodeAtMouse);
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