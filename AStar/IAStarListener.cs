using System;

namespace AStar
{
    /// <summary>
    /// An interface used by AStar.cs to communicate intermediate pathfinding steps with users.
    /// </summary>
    public interface IAStarListener<T> : IPathFindingListener<T> where T : IEquatable<T>
    {
        void SetGValue(T cell, double value);
        void SetFValue(T cell, double value);
    }
}