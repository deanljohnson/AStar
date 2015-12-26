using System;
using System.Collections.Generic;
using System.Linq;
using PriorityQueue;

namespace AStar
{
    public static class AStar<T> where T : IEquatable<T>
    {
        public delegate double DistanceFunc(T a, T b);

        public delegate List<T> GetNeighbors(T t);

        static AStar()
        {
            HeuristicScale = 1.0;
        }

        private static Dictionary<T, T> m_Parents { get; set; }
        private static Dictionary<T, double> m_GValues { get; set; }
        //We keep Closed and Open public for visualization demos
        public static HashSet<T> Closed { get; private set; }
        public static PriorityQueue<T, double> Open { get; private set; }
        // ReSharper disable once StaticMemberInGenericType
        public static double HeuristicScale { get; set; }

        /// <summary>
        ///     Returns an efficient path from start to end. If no path exists, algorithm will run forever.
        /// </summary>
        /// <param name="start">The start point</param>
        /// <param name="end">The end point</param>
        /// <param name="getNeighbors">A function returning the neighbors of any node</param>
        /// <param name="distanceFunc">A function returning the approximate distance between two nodes</param>
        /// <returns></returns>
        public static Stack<T> PathFind(T start, T end, GetNeighbors getNeighbors,
            DistanceFunc distanceFunc)
        {
            if (start.Equals(end))
            {
                var ret = new Stack<T>();
                ret.Push(end);
                return ret;
            }

            Closed = new HashSet<T>();
            Open = new PriorityQueue<T, double>();
            Open.Enqueue(start, 0);

            m_GValues = new Dictionary<T, double> {{start, 0.0}, {end, 0.0}};
            m_Parents = new Dictionary<T, T>();

            while (Open.Any())
            {
                //Take the most promising node
                var currentNode = Open.Dequeue();

                if (currentNode == null || currentNode.Equals(end))
                {
                    return ConstructBestPath(start, end);
                }

                Closed.Add(currentNode);

                var neighbors = CallGetNeighbors(currentNode, getNeighbors);
                foreach (var i in neighbors)
                {
                    if (Closed.Contains(i))
                        continue;

                    var currentG = m_GValues[currentNode] + distanceFunc(currentNode, i);
                    var potentialF = currentG + (distanceFunc(i, end)*HeuristicScale);

                    if (!Open.Contains(i))
                    {
                        SetParent(i, currentNode);
                        m_GValues[i] = currentG;
                        Open.Enqueue(i, potentialF);
                    }
                    else if (potentialF < Open.GetPriority(i))
                    {
                        SetParent(i, currentNode);
                        m_GValues[i] = currentG;
                        Open.SetPriority(i, potentialF);
                    }
                }
            }

            return new Stack<T>();
        }

        /// <summary>
        ///     Sets the parent of the given node. If the node already has a parent assigned, the previous parent is overwritten
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        private static void SetParent(T node, T parent)
        {
            if (m_Parents.ContainsKey(node))
            {
                m_Parents[node] = parent;
            }
            else
            {
                m_Parents.Add(node, parent);
            }
        }

        /// <summary>
        ///     Calls the GetNeighbors functions, creating G values for the node if the neighbor has not been seen before
        /// </summary>
        /// <param name="node"></param>
        /// <param name="getNeighbors"></param>
        /// <returns></returns>
        private static IEnumerable<T> CallGetNeighbors(T node, GetNeighbors getNeighbors)
        {
            var neighbors = getNeighbors(node);

            foreach (var n in neighbors.Where(n => !m_GValues.ContainsKey(n)))
            {
                m_GValues.Add(n, 0.0);
            }

            return neighbors;
        }

        /// <summary>
        ///     Builds the found path from start to end
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static Stack<T> ConstructBestPath(T start, T end)
        {
            var bestPath = new Stack<T>();
            bestPath.Push(end);

            var currentNode = m_Parents[end];
            bestPath.Push(currentNode);

            while (!currentNode.Equals(start))
            {
                currentNode = m_Parents[currentNode];
                bestPath.Push(currentNode);
            }

            return bestPath;
        }
    }
}