using System;
using AStar;
using SFML.System;

namespace AStarSimulation
{
    class SolverFactory
    {
        public void CreateSolver(AlgorithmType algoType, IIndexedPathfindingMap map, double heuristic, 
            out IPathFinder<Vector2i> pathFinder, out IPathFindingListener<Vector2i> listener)
        {
            switch (algoType)
            {
                case AlgorithmType.AStar:
                    listener = new AStarListener();
                    pathFinder = new AStar<Vector2i>(map.NeighborsOfCell, map.DistanceEstimate, heuristic)
                    {
                        Listener = (IAStarListener<Vector2i>)listener
                    };
                    return;
                case AlgorithmType.Dijkstra:
                    listener = new DijkstraListener();
                    pathFinder = new Dijkstra<Vector2i>(map.NeighborsOfCell, map.DistanceEstimate)
                    {
                        Listener = listener
                    };
                    return;
            }

            throw new Exception($"Unrecognized algorithm type: {algoType}");
        }
    }
}
