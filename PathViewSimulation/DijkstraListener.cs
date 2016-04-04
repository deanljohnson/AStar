using System.Collections.Generic;
using PathView;
using SFML.System;

namespace PathViewSimulation
{
    class DijkstraListener : IPathFindingListener<Vector2i>
    {
        public HashSet<Vector2i> Open { get; } = new HashSet<Vector2i>();
        public HashSet<Vector2i> Closed { get; } = new HashSet<Vector2i>();
        public Dictionary<Vector2i, Vector2i> Parents { get; } = new Dictionary<Vector2i, Vector2i>();

        public void Reset()
        {
            Open.Clear();
            Closed.Clear();
        }

        public void SetOpen(Vector2i cell)
        {
            Open.Add(cell);
            Closed.Remove(cell);
        }

        public void SetClosed(Vector2i cell)
        {
            Open.Remove(cell);
            Closed.Add(cell);
        }

        public void SetParent(Vector2i child, Vector2i parent)
        {
            Parents[child] = parent;
        }
    }
}
