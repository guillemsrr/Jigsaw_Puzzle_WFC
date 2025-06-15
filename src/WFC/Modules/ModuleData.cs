using Godot;
using GodotTest.WFC.Adjacency;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Modules
{
    public partial class ModuleData: Node
    {
        public string Key;
        public int Number;
        public int Frequency = 1;
        public Rotation Rotation;
        public PackedScene PackedScene;
        public AdjacencyData AdjacencyData;

        public ModuleData(ModuleData moduleData)
        {
            Key = moduleData.Key;
            Number = moduleData.Number;
            Frequency = moduleData.Frequency;
            Rotation = moduleData.Rotation;
            PackedScene = moduleData.PackedScene;
        }

        public ModuleData(PackedScene packedScene, AdjacencyData adjacencyData, Rotation rotation, int numberModuleData = 0)
        {
            Key = numberModuleData + "_" + packedScene.ResourceName + "_" + rotation;
            Number = numberModuleData;
            Rotation = rotation;
            PackedScene = packedScene;
            AdjacencyData = adjacencyData;
        }
    }
}