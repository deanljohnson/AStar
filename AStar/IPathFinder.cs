using System.Collections.Generic;

namespace AStar
{
    public interface IPathFinder<T>
    {
        Stack<T> PathFind(T start, T end);
        Stack<T> PathFindOneStep(T start, T end);
    }
}