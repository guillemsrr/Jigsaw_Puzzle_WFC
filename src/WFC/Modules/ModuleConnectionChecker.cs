using GodotTest.WFC.Adjacency;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Modules
{
    public static class ModuleConnectionChecker
    {
        public static bool CanConnect(ModuleData moduleData, ModuleData possibleNeighbor, Direction direction)
        {
            Direction neighborDirection = Directions.FlipDirection(direction);
            neighborDirection = Directions.RotateDirection(neighborDirection, possibleNeighbor.Rotation);
            direction = Directions.RotateDirection(direction, moduleData.Rotation);

            AdjacencyType adjacencyType1 = moduleData.AdjacencyData.FacesByDirection[direction];
            AdjacencyType adjacencyType2 = possibleNeighbor.AdjacencyData.FacesByDirection[neighborDirection];
            bool canBeAdjacent = AdjacencyRules.CanBeAdjacent(adjacencyType1, adjacencyType2);
            return canBeAdjacent;
        }
    }
}