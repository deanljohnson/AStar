using System;
using System.Collections.Generic;

namespace AStar
{
    public interface IPathFindingListener<T> where T : IEquatable<T>
    {
        HashSet<T> Open { get; }
        HashSet<T> Closed { get; } 
        void Reset();
        void SetOpen(T cell);
        void SetClosed(T cell);
        void SetParent(T child, T parent);
    }
}