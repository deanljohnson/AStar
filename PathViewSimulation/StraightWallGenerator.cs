namespace PathViewSimulation
{
    class StraightWallGenerator : IWallGenerator
    {
        public void GenerateWalls(IIndexedPathfindingMap map, int count)
        {
            for (var i = 0; i < count; i++)
            {
                GenerateWall(map);
            }
        }

        private void GenerateWall(IIndexedPathfindingMap map)
        {
            var start = map.RandomOpenCell();
            var end = map.RandomOpenCell();

            //don't allow walls to start and end in the same place.
            while (start == end) { end = map.RandomOpenCell(); }

            var cellsInLine = map.CellsInLine(start, end);

            foreach (var cell in cellsInLine)
            {
                map.Set(cell, CellState.Wall);
                map.Set(map.GetNeighbors(cell), CellState.Wall);
            }
        }
    }
}
