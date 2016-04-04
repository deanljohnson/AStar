using System;

namespace PathViewSimulation
{
    public class PathfindingData
    {
        /// <summary>
        /// The total number of nodes in the graph of the most recent pathfinding instance
        /// </summary>
        public int GraphSize { get; set; }

        /// <summary>
        /// The average number of nodes visited per pathfinding instance
        /// </summary>
        public float AverageNodesVisited => TotalNodesVisited/(float)TotalPathsComputed;
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
        public long TotalPathsComputed { get; set; }
        /// <summary>
        /// The length of the path found during the most recent instance of path finding
        /// </summary>
        public int PathLength { get; set; }

        /// <summary>
        /// The average time spent in pathfinding, in ms
        /// </summary>
        public float AverageTime => (float) (TotalPathfindingTime.TotalMilliseconds / TotalPathsComputed);

        /// <summary>
        /// The total time that has been spent pathfinding, in ms
        /// </summary>
        public TimeSpan TotalPathfindingTime { get; set; }
        /// <summary>
        /// The time spent in the most recent instance of pathfinding, in ms
        /// </summary>
        public TimeSpan PathfindingTime { get; set; }

        public void Reset()
        {
            GraphSize = 0;
            PathLength = 0;
            NodesVisited = 0;
            PathfindingTime = new TimeSpan();

            TotalPathsComputed = 0;
            TotalNodesVisited = 0;
            TotalPathfindingTime = new TimeSpan();
        }
    }
}
