namespace AStarSimulation
{
    public interface IWallGenerator
    {
        /// <summary>
        /// Generates the given number of walls on the given IIndexedPathfindingMap
        /// </summary>
        void GenerateWalls(IIndexedPathfindingMap map, int count);
    }
}