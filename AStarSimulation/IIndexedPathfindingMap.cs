using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AStarSimulation
{
    interface IIndexedPathfindingMap : Drawable
    {
        /// <summary>
        /// Return the number of cells in this map
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns the x/y dimensions of the map. Does not necessarily have geometric significance.
        /// </summary>
        Vector2i Dimensions { get; }

        /// <summary>
        /// Sets the state of the cell at index i to s
        /// </summary>
        void Set(Vector2i i, CellState s);

        /// <summary>
        /// Sets the state of the given indices to s
        /// </summary>
        void Set(IEnumerable<Vector2i> i, CellState s);

        /// <summary>
        /// Sets all cell states to the given state
        /// </summary>
        void SetAll(CellState s);

        /// <summary>
        /// Returns whether or not a given index is in the given state
        /// </summary>
        bool Is(Vector2i i, CellState s);

        /// <summary>
        /// Returns a random cell that does not have a state of CellState.Wall
        /// </summary>
        Vector2i RandomOpenCell();

        /// <summary>
        /// Returns the index that most applies to the given pixel
        /// </summary>
        Vector2i PixelToIndex(Vector2i p);

        double DistanceEstimate(Vector2i a, Vector2i b);
        List<Vector2i> NeighborsOfCell(Vector2i current);
    }
}
