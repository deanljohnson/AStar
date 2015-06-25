using System;
using System.Collections.Generic;
using System.Linq;

namespace AStar
{
    public static class AStar<T>
    {
        private static Dictionary<T, T> Parents { get; set; }
        private static Dictionary<T, double> FValues { get; set; }
        private static Dictionary<T, double> GValues { get; set; }

        //We keep Closed and Open public for visualization demos
        public static List<T> Closed { get; private set; }
        public static List<T> Open { get; private set; }

        public static double HeuristicScale { private get; set; }

        static AStar()
        {
            HeuristicScale = 1.0;
        }

        /// <summary>
        /// Returns an efficient path from start to end. If no path exists, algorithm will run forever.
        /// </summary>
        /// <param name="start">The start point</param>
        /// <param name="end">The end point</param>
        /// <param name="GetNeighbors">A function returning the neighbors of any node</param>
        /// <param name="DistanceFunc">A function returning the approximate distance between two nodes</param>
        /// <returns></returns>
        public static Stack<T> PathFind(T start, T end, Func<T, List<T>> GetNeighbors,
            Func<T, T, double> DistanceFunc)
        {
            Closed = new List<T>();
            Open = new List<T> { start };
            FValues = new Dictionary<T, double> { { start, 0.0 }, { end, 0.0 } };
            GValues = new Dictionary<T, double> { { start, 0.0 }, { end, 0.0 } };
            Parents = new Dictionary<T, T>();

            while(Open.Any())
            {
                var currentNode = (T) LowestFScore(Open);

                if (currentNode == null || Equals(currentNode, end))
                {
                    return ConstructBestPath(start, end);
                }

                Closed.Add(currentNode);
                Open.Remove(currentNode);

                var neighbors = CallGetNeighbors(currentNode, GetNeighbors);
                foreach (var i in neighbors)
                {
                    if (Closed.Contains(i))
                        continue;

                    var currentG = GValues[currentNode] + DistanceFunc(currentNode, i);
                    var openHasI = Open.Contains(i);

                    if (!openHasI || currentG < GValues[i])
                    {
                        SetParent(i, currentNode);
                        GValues[i] = currentG;
                        FValues[i] = currentG + (DistanceFunc(i, end) * HeuristicScale);

                        if (!openHasI)
                        {
                            Open.Add(i);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Returns an efficient path from start to and end point with a minimal path length
        /// </summary>
        /// <param name="start">The start point</param>
        /// <param name="endPoints">A list of acceptable end points</param>
        /// <param name="GetNeighbors">A function returning the neighbors of any node</param>
        /// <param name="DistanceFunc">A function returning the approximate distance between two nodes</param>
        /// <returns></returns>
        public static Stack<T> PathFind(T start, List<T> endPoints, Func<T, List<T>> GetNeighbors,
            Func<T, T, double> DistanceFunc)
        {
            Closed = new List<T>();
            Open = new List<T> { start };

            FValues = new Dictionary<T, double> { { start, 0.0 } };
            GValues = new Dictionary<T, double> { { start, 0.0 } };

            foreach (var endPoint in endPoints)
            {
                FValues.Add(endPoint, 0.0);
                GValues.Add(endPoint, 0.0);
            }

            Parents = new Dictionary<T, T>();

            while (Open.Any())
            {
                var currentNode = (T)LowestFScore(Open);

                if (currentNode == null || endPoints.Any(e => e.Equals(currentNode)))
                {
                    return ConstructBestPath(start, endPoints[endPoints.IndexOf(currentNode)]);
                }

                Closed.Add(currentNode);
                Open.Remove(currentNode);

                var neighbors = CallGetNeighbors(currentNode, GetNeighbors);
                foreach (var i in neighbors)
                {
                    if (Closed.Contains(i))
                        continue;

                    var currentG = GValues[currentNode] + DistanceFunc(currentNode, i);
                    var openHasI = Open.Contains(i);

                    if (!openHasI || currentG < GValues[i])
                    {
                        SetParent(i, currentNode);
                        GValues[i] = currentG;
                        FValues[i] = currentG + (DistanceToClosest(i, endPoints, DistanceFunc) * HeuristicScale);

                        if (!openHasI)
                        {
                            Open.Add(i);
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Sets the parent of the given node. If the node already has a parent assigned, the previous parent is overwritten
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        private static void SetParent(T node, T parent)
        {
            if (Parents.ContainsKey(node))
            {
                Parents[node] = parent;
            }
            else
            {
                Parents.Add(node, parent);
            }
        }

        /// <summary>
        /// Calls the GetNeighbors functions, creating F and G values for the node if the node has not been seen before
        /// </summary>
        /// <param name="node"></param>
        /// <param name="GetNeighbors"></param>
        /// <returns></returns>
        private static IEnumerable<T> CallGetNeighbors(T node, Func<T, List<T>> GetNeighbors)
        {
            var neighbors = GetNeighbors(node);

            foreach (var n in neighbors)
            {
                if (!FValues.ContainsKey(n))
                {
                    FValues.Add(n, 0.0);
                }
                if (!GValues.ContainsKey(n))
                {
                    GValues.Add(n, 0.0);
                }
            }

            return neighbors;
        }

        /// <summary>
        /// Finds the closest node and returns that distance
        /// </summary>
        /// <param name="node"></param>
        /// <param name="endPoints"></param>
        /// <param name="DistanceFunc"></param>
        /// <returns></returns>
        private static double DistanceToClosest(T node, IEnumerable<T> endPoints, Func<T, T, double> DistanceFunc)
        {
            return endPoints.Select(endPoint => DistanceFunc(node, endPoint)).Concat(new[] {double.MaxValue}).Min();
        }

        private static object LowestFScore(List<T> nodes)
        {
            if (nodes.Count == 0)
                return null;

            nodes.Sort((x, y) => Comparer<double>.Default.Compare(FValues[x], FValues[y]));
            return nodes[0];
        }

        /// <summary>
        /// Builds the found path from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static Stack<T> ConstructBestPath(T start, T end)
        {
            var bestPath = new Stack<T>();

            var currentNode = Parents[end];
            bestPath.Push(currentNode);

            while (!Equals(currentNode, start) && currentNode != null)
            {
                currentNode = Parents[currentNode];
                bestPath.Push(currentNode);
            }

            return bestPath;
        }
    }
}