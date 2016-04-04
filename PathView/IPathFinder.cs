using System.Collections.Generic;

namespace PathView
{
    public interface IPathFinder<T>
    {
        Stack<T> PathFind(T start, T end);
        Stack<T> PathFindOneStep(T start, T end);
    }
}