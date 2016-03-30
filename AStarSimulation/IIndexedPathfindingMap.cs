using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace AStarSimulation
{
    public interface IIndexedPathfindingMap : Drawable
    {
        /// <summary>
        /// Returns whether or not this map is uniform,
        /// ie. the cost between any neighboring cell is constant
        /// </summary>
        bool IsUniform { get; }

        /// <summary>
        /// Return the number of cells in this map
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Sets the state of the cell at i to s
        /// </summary>
        void Set(Vector2i i, CellState s);

        /// <summary>
        /// Sets the state of the given indices to s
        /// </summary>
        void Set(IEnumerable<Vector2i> indices, CellState s);

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

        /// <summary>
        /// Returns an estimation of the path length between a and b.
        /// Should never over-estimate.
        /// </summary>
        double DistanceEstimate(Vector2i a, Vector2i b);

        /// <summary>
        /// Returns neighbors of the given cell that are traversable
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        List<Vector2i> NeighborsOfCell(Vector2i current);

        /// <summary>
        /// Returns the cells in a "line" from a to b. What exactly a line is can vary
        /// by the type of grid, but it can generally be thought of as the straightest
        ///  between two points.
        /// </summary>
        List<Vector2i> CellsInLine(Vector2i a, Vector2i b);
    }
}
