using System;

namespace AStarSimulation
{
    public class PathfindingData
    {
        private bool? m_ConsolePresent;
        public bool ConsolePresent
        {
            get
            {
                if (m_ConsolePresent == null)
                {
                    m_ConsolePresent = true;
                    try { var windowHeight = Console.WindowHeight; }
                    catch { m_ConsolePresent = false; }
                }
                return m_ConsolePresent.Value;
            }
        }

        /// <summary>
        /// The total number of nodes in the graph of the most recent pathfinding instance
        /// </summary>
        public int GraphSize { get; set; }

        /// <summary>
        /// The average number of nodes visited per pathfinding instance
        /// </summary>
        public float AverageNodesVisited => TotalNodesVisited/(float)PathsComputed;
        /// <summary>
        /// The total number of nodes visited
        /// </summary>
        public long TotalNodesVisited { get; set; }
        /// <summary>
        /// The number of nodes visited in the most recent instance of path finding
        /// </summary>
        public long NodesVisited { get; set; }
        /// <summary>
        /// The total number of paths that have been computed
        /// </summary>
        public long PathsComputed { get; set; }
        /// <summary>
        /// The length of the path found during the most recent instance of path finding
        /// </summary>
        public int PathLength { get; set; }

        /// <summary>
        /// The average time spent in pathfinding
        /// </summary>
        public float AverageTime => TotalPathfindingTime/(float) PathsComputed;
        /// <summary>
        /// The total time that has been spent pathfinding, in ms
        /// </summary>
        public long TotalPathfindingTime { get; set; }
        /// <summary>
        /// The time spent in the most recent instance of pathfinding, in ms
        /// </summary>
        public long PathfindingTime { get; set; }

        public void OutputDataToConsole()
        {
            if (!ConsolePresent) return;
            Console.Clear();
            Console.WriteLine("Graph Size: {0}", GraphSize);
            Console.WriteLine("Time Taken: " + PathfindingTime + "ms");
            Console.WriteLine("Path Length: " + PathLength);
            Console.WriteLine("Nodes Visited: " + NodesVisited);
            Console.WriteLine("Paths Computed: " + PathsComputed);
            Console.WriteLine("Average Nodes Visited: " + AverageNodesVisited);
            Console.WriteLine("Average Time: " + AverageTime + "ms");
            Console.WriteLine("Total Time in Pathfinding: " + TotalPathfindingTime / 1000f + "s");
        }
    }
}
