using System;
using System.Collections.Generic;
using PathViewSimulation.Grids.Hexagon;
using PathViewSimulation.Grids.Square;
using SFML.Graphics;
using SFML.System;
using SFNetHex;

namespace PathViewSimulation
{
    static class IndexedPathfindingMapFactory
    {
        public static IIndexedPathfindingMap BuildMap(GridType type, Vector2i nodeSize, Vector2u bounds,
            Dictionary<CellState, Color> colorMap)
        {
            switch (type)
            {
                case GridType.SquareEuclidean:
                    var gridEuc = new Vector2i((int)(bounds.X / nodeSize.X), (int)(bounds.Y / nodeSize.Y));
                    return new SquareGrid(nodeSize, gridEuc, colorMap)
                    {
                        UseManhattanMetric = false
                    };
                case GridType.SquareManhattan:
                    var gridMan = new Vector2i((int)(bounds.X / nodeSize.X), (int)(bounds.Y / nodeSize.Y));
                    return new SquareGrid(nodeSize, gridMan, colorMap)
                    {
                        UseManhattanMetric = true
                    };
                case GridType.Hex:
                    var floatHexSize = new Vector2f(nodeSize.X, nodeSize.Y);
                    var testHex = new HexShape(new Layout(Orientation.Flat, floatHexSize, new Vector2f(0, 0)));
                    var size = new Vector2f(testHex.GetLocalBounds().Width, testHex.GetLocalBounds().Height);
                    //We subtract one to handle the center hex
                    var vertRadius = bounds.Y / (2f * size.Y) - 1;
                    var horizRadius = bounds.X / (2f * size.X) - 1;

                    return new HexGrid((int)Math.Min(vertRadius, horizRadius), Orientation.Flat, floatHexSize, colorMap)
                    { Position = new Vector2f(bounds.X / 2f, bounds.Y / 2f) };
            }

            throw new ArgumentException("Cannot build a grid of type " + type);
        }
    }
}
