using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PriorityQueue;

namespace AStar
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

        //We keep Closed and Open public for demo purposes
        public HashSet<T> Closed { get; private set; }
        public PriorityQueue<T, double> Open { get; private set; }
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

            while (Open.Any())
            {
                //Take the most promising node
                var currentNode = Open.Dequeue();

                if (currentNode == null || currentNode.Equals(end))
                {
                    return ConstructBestPath(start, end);
                }

                Closed.Add(currentNode);
                Listener?.SetClosed(currentNode);

                var neighbors = CallGetNeighbors(currentNode);
                foreach (var neighbor in neighbors)
                {
                    if (Closed.Contains(neighbor))
                        continue;

                    var neighborG = m_GValues[currentNode] + m_DistanceFunc(currentNode, neighbor);
                    var potentialF = neighborG + (m_DistanceFunc(neighbor, end) * HeuristicScale);

                    if (!Open.Contains(neighbor))
                    {
                        SetParent(neighbor, currentNode);
                        m_GValues[neighbor] = neighborG;
                        Open.Enqueue(neighbor, potentialF);

                        if (Listener == null) continue;

                        Listener.SetOpen(neighbor);
                        Listener.SetParent(neighbor, currentNode);
                        Listener.SetGValue(neighbor, neighborG);
                        Listener.SetFValue(neighbor, potentialF);
                    }
                    else if (potentialF < Open.GetPriority(neighbor))
                    {
                        SetParent(neighbor, currentNode);
                        m_GValues[neighbor] = neighborG;
                        Open.SetPriority(neighbor, potentialF);

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
            if (Open == null)
            {
                InitializeCollections(start, end);
            }

            Debug.Assert(Open != null, "Collections were not initialized correctly");

            //Take the most promising node
            var currentNode = Open.Dequeue();

            if (currentNode == null || currentNode.Equals(end))
            {
                return ConstructBestPath(start, end);
            }

            Closed.Add(currentNode);
            Listener?.SetClosed(currentNode);

            var neighbors = CallGetNeighbors(currentNode);
            foreach (var neighbor in neighbors)
            {
                if (Closed.Contains(neighbor))
                    continue;

                var neighborG = m_GValues[currentNode] + m_DistanceFunc(currentNode, neighbor);
                var potentialF = neighborG + (m_DistanceFunc(neighbor, end) * HeuristicScale);

                if (!Open.Contains(neighbor))
                {
                    SetParent(neighbor, currentNode);
                    m_GValues[neighbor] = neighborG;
                    Open.Enqueue(neighbor, potentialF);

                    if (Listener == null) continue;

                    Listener.SetOpen(neighbor);
                    Listener.SetParent(neighbor, currentNode);
                    Listener.SetGValue(neighbor, neighborG);
                    Listener.SetFValue(neighbor, potentialF);
                }
                else if (potentialF < Open.GetPriority(neighbor))
                {
                    SetParent(neighbor, currentNode);
                    m_GValues[neighbor] = neighborG;
                    Open.SetPriority(neighbor, potentialF);

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
            Closed = new HashSet<T>();
            Open = new PriorityQueue<T, double>();
            Open.Enqueue(start, 0);

            m_GValues = new Dictionary<T, double> { { start, 0.0 }, { end, 0.0 } };
            m_Parents = new Dictionary<T, T>();

            Listener?.Reset();
        }

        /// <summary>
        ///     Sets the parent of the given node. If the node already has a parent assigned, the previous parent is overwritten
        /// </summary>
        private void SetParent(T node, T parent)
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