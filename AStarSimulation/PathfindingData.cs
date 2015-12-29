using System;

namespace AStarSimulation
{
    internal struct PathfindingData
    {
        public double HeuristicUsed { get; set; }
        public int GraphSize { get; set; }

        public long TotalNodesVisited { get; set; }
        public long NodesVisited { get; set; }
        public long PathsComputed { get; set; }
        public int PathLength { get; set; }

        public long TotalPathfindingTime { get; set; }
        public long PathfindingTime { get; set; }

        public void OutputData()
        {
            Console.Clear();
            Console.WriteLine("Heuristic: " + HeuristicUsed);
            Console.WriteLine("Graph Size: {0}", GraphSize);
            Console.WriteLine("Time Taken: " + PathfindingTime + "ms");
            Console.WriteLine("Path Length: " + PathLength);
            Console.WriteLine("Nodes Visited: " + NodesVisited);
            Console.WriteLine("Paths Computed: " + PathsComputed);
            Console.WriteLine("Average Nodes Visited: " + TotalNodesVisited / PathsComputed);
            Console.WriteLine("Average Time: " + TotalPathfindingTime / (float)PathsComputed + "ms");
            Console.WriteLine("Total Time in Pathfinding: " + TotalPathfindingTime / 1000f + "s");
        }
    }
}
