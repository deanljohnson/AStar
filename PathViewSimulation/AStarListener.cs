using System.Collections.Generic;
using PathView;
using SFML.System;

namespace PathViewSimulation
{
    public class AStarListener : IAStarListener<Vector2i>
    {
        public HashSet<Vector2i> Open { get; } = new HashSet<Vector2i>();
        public HashSet<Vector2i> Closed { get; } = new HashSet<Vector2i>();
        public Dictionary<Vector2i, double> GValues { get; } = new Dictionary<Vector2i, double>();
        public Dictionary<Vector2i, double> FValues { get; } = new Dictionary<Vector2i, double>();
        public Dictionary<Vector2i, Vector2i> Parents { get; } = new Dictionary<Vector2i, Vector2i>();

        public void Reset()
        {
            Open.Clear();
            Closed.Clear();
            GValues.Clear();
            FValues.Clear();
            Parents.Clear();
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

        public void SetGValue(Vector2i cell, double value)
        {
            GValues[cell] = value;
        }

        public void SetFValue(Vector2i cell, double value)
        {
            FValues[cell] = value;
        }

        public void SetParent(Vector2i child, Vector2i parent)
        {
            Parents[child] = parent;
        }
    }
}
