using System;
using System.Collections.Generic;
using System.Linq;
using PriorityQueue;

namespace AStar
{
    public class Dijkstra<T> : IPathFinder<T> where T : IEquatable<T>
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
        private Dictionary<T, double> m_Distances { get; set; }
        private HashSet<T> m_Closed { get; set; }
        private PriorityQueue<T, double> m_Open { get; set; }

        public IPathFindingListener<T> Listener { get; set; }

        public Dijkstra(Func<T, IList<T>> getNeighbors, Func<T, T, double> distanceFunc)
        {
            m_NeighborsFunc = getNeighbors;
            m_DistanceFunc = distanceFunc;
        }

        public Stack<T> PathFind(T start, T end)
        {
            InitializeCollections(start, end);
            Listener?.Reset();

            while (m_Open.Any())
            {
                var current = m_Open.Dequeue();

                if (current.Equals(end))
                {
                    return ConstructBestPath(start, end);
                }

                m_Closed.Add(current);
                Listener?.SetClosed(current);

                foreach (var neighbor in m_NeighborsFunc(current).Where(n => !m_Closed.Contains(n)))
                {
                    var d = m_Distances[current] + m_DistanceFunc(current, neighbor);

                    if (!m_Open.Contains(neighbor))
                    {
                        m_Parents[neighbor] = current;
                        m_Open.Enqueue(neighbor, d);
                        m_Distances.Add(neighbor, d);

                        if (Listener == null) continue;

                        Listener.SetOpen(neighbor);
                        Listener.SetParent(neighbor, current);
                    }
                    else if (d < m_Distances[neighbor])
                    {
                        m_Distances[neighbor] = d;
                        m_Parents[neighbor] = current;
                        Listener?.SetParent(neighbor, current);

                        if (m_Open.Contains(neighbor))
                        {
                            m_Open.SetPriority(neighbor, d);
                        }
                        else
                        {
                            m_Open.Enqueue(neighbor, d);
                            Listener?.SetOpen(neighbor);
                        }
                    }
                }
            }

            return new Stack<T>();
        }

        public Stack<T> PathFindOneStep(T start, T end)
        {
            return new Stack<T>();
        }

        private void InitializeCollections(T start, T end)
        {
            m_Parents = new Dictionary<T, T>();

            m_Distances = new Dictionary<T, double>
            {
                { start, 0.0 }
            };

            m_Open = new PriorityQueue<T, double>();
            m_Open.Enqueue(start, 0);
            m_Closed = new HashSet<T>();
        }

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
