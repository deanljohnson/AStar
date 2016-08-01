using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PriorityQueue;

namespace PathView
{
    public class AStar<T> : IPathFinder<T> where T : IEquatable<T>
    {
        /// <summary>
        /// A function that returns the neighbors of a given node.
        /// </summary>
        private Func<T, IList<T>> m_NeighborsFunc { get; }

        /// <summary>
        /// A function that return the estimated distance between two nodes.
        /// </summary>
        private Func<T, T, double> m_DistanceFunc { get; }

        private Dictionary<T, T> m_Parents { get; set; }
        private Dictionary<T, double> m_GValues { get; set; }
        private HashSet<T> m_Closed { get; set; }
        private PriorityQueue<T, double> m_Open { get; set; }

        // ReSharper disable once StaticMemberInGenericType
        public double HeuristicScale { get; set; }
        public IAStarListener<T> Listener { get; set; }

        public AStar(Func<T, IList<T>> getNeighbors, Func<T, T, double> distanceFunc, double heuristic = 1.0)
        {
            HeuristicScale = heuristic;
            m_NeighborsFunc = getNeighbors;
            m_DistanceFunc = distanceFunc;
        }

        /// <summary>
        ///     Returns an efficient path from start to end. If no path exists, algorithm will run forever.
        /// </summary>
        public Stack<T> PathFind(T start, T end)
        {
            if (start.Equals(end))
            {
                var ret = new Stack<T>();
                ret.Push(end);
                return ret;
            }

            InitializeCollections(start, end);
            Listener?.Reset();

            while (m_Open.Any())
            {
                //Take the most promising node
                var currentNode = m_Open.Dequeue();

                if (currentNode == null || currentNode.Equals(end))
                {
                    return ConstructBestPath(start, end);
                }

                m_Closed.Add(currentNode);
                Listener?.SetClosed(currentNode);

                var neighbors = CallGetNeighbors(currentNode);
                foreach (var neighbor in neighbors)
                {
                    if (m_Closed.Contains(neighbor))
                        continue;

                    var neighborG = m_GValues[currentNode] + m_DistanceFunc(currentNode, neighbor);
                    var potentialF = neighborG + (m_DistanceFunc(neighbor, end) * HeuristicScale);

                    if (!m_Open.Contains(neighbor))
                    {
                        m_Parents[neighbor] = currentNode;
                        m_GValues[neighbor] = neighborG;
                        m_Open.Enqueue(neighbor, potentialF);

                        if (Listener == null) continue;

                        Listener.SetOpen(neighbor);
                        Listener.SetParent(neighbor, currentNode);
                        Listener.SetGValue(neighbor, neighborG);
                        Listener.SetFValue(neighbor, potentialF);
                    }
                    else if (potentialF < m_Open.GetPriority(neighbor))
                    {
                        m_Parents[neighbor] = currentNode;
                        m_GValues[neighbor] = neighborG;
                        m_Open.SetPriority(neighbor, potentialF);

                        if (Listener == null) continue;

                        Listener.SetParent(neighbor, currentNode);
                        Listener.SetGValue(neighbor, neighborG);
                        Listener.SetFValue(neighbor, potentialF);
                    }
                }
            }

            return new Stack<T>();
        }

        /// <summary>
        /// Expands one node, examining it's neighbors. Returns null until a path is found.
        /// </summary>
        public Stack<T> PathFindOneStep(T start, T end)
        {
            if (m_Open == null)
            {
                InitializeCollections(start, end);
                Listener?.Reset();
            }

            Debug.Assert(m_Open != null, "Collections were not initialized correctly");

            if (!m_Open.Any())
                return null;
            //Take the most promising node
            var currentNode = m_Open.Dequeue();

            if (currentNode == null || currentNode.Equals(end))
            {
                return ConstructBestPath(start, end);
            }

            m_Closed.Add(currentNode);
            Listener?.SetClosed(currentNode);

            var neighbors = CallGetNeighbors(currentNode);
            foreach (var neighbor in neighbors)
            {
                if (m_Closed.Contains(neighbor))
                    continue;

                var neighborG = m_GValues[currentNode] + m_DistanceFunc(currentNode, neighbor);
                var potentialF = neighborG + (m_DistanceFunc(neighbor, end) * HeuristicScale);

                if (!m_Open.Contains(neighbor))
                {
                    m_Parents[neighbor] = currentNode;
                    m_GValues[neighbor] = neighborG;
                    m_Open.Enqueue(neighbor, potentialF);

                    if (Listener == null) continue;

                    Listener.SetOpen(neighbor);
                    Listener.SetParent(neighbor, currentNode);
                    Listener.SetGValue(neighbor, neighborG);
                    Listener.SetFValue(neighbor, potentialF);
                }
                else if (potentialF < m_Open.GetPriority(neighbor))
                {
                    m_Parents[neighbor] = currentNode;
                    m_GValues[neighbor] = neighborG;
                    m_Open.SetPriority(neighbor, potentialF);

                    if (Listener == null) continue;

                    Listener.SetParent(neighbor, currentNode);
                    Listener.SetGValue(neighbor, neighborG);
                    Listener.SetFValue(neighbor, potentialF);
                }
            }

            return null;
        }

        private void InitializeCollections(T start, T end)
        {
            m_Closed = new HashSet<T>();
            m_Open = new PriorityQueue<T, double>();
            m_Open.Enqueue(start, 0);

            m_GValues = new Dictionary<T, double> { { start, 0.0 }, { end, 0.0 } };
            m_Parents = new Dictionary<T, T>();
        }

        /// <summary>
        ///     Calls the GetNeighbors functions, creating G values for the node if the neighbor has not been seen before
        /// </summary>
        private IEnumerable<T> CallGetNeighbors(T node)
        {
            var neighbors = m_NeighborsFunc(node);

            foreach (var n in neighbors.Where(n => !m_GValues.ContainsKey(n)))
            {
                m_GValues.Add(n, 0.0);
            }

            return neighbors;
        }

        /// <summary>
        ///     Builds the found path from start to end.
        /// </summary>
        private Stack<T> ConstructBestPath(T start, T end)
        {
            var bestPath = new Stack<T>();

            //Work backwards from the end node.
            var currentNode = end;
            while (!currentNode.Equals(start))
            {
                bestPath.Push(currentNode);
                currentNode = m_Parents[currentNode];
            }

            return bestPath;
        }
    }
}