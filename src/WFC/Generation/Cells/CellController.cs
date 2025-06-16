using System.Collections.Generic;
using Godot;
using GodotTest.WFC.Modules;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Generation.Cells
{
    public class CellController
    {
        private Node3D _parent;
        private float _entropyNoise;
        private float _size;

        public delegate void CellPropagated(CellController cell);

        public event CellPropagated CellPropagatedEvent;

        public Vector3I Position { get; }
        public bool IsCollapsed => CellData.CollapsedModuleData != null;
        public bool OnlyOnePossibility => CellData.PossibleModules.Count == 1;
        public bool CanCollapse => !IsCollapsed && HasPossibilities;
        public bool HasPossibilities => CellData.PossibleModules.Count != 0;

        public List<ModuleData> Possibilities => CellData.PossibleModules;

        public CellData CellData { get; set; }

        public CellController(Node3D parent, ModuleData[] AdjacencyDatas, Vector3I position, float size)
        {
            _parent = parent;
            Position = position;
            _size = size;

            CellData = new CellData(AdjacencyDatas);
        }

        public void Collapse()
        {
            ModuleData moduleData = GetWeightedRandomModule();
            CellData.CollapsedModuleData = moduleData;
        }

        public Node3D InstantiateModule()
        {
            Vector3 position = new Vector3(Position.X, Position.Y, Position.Z);
            position *= _size;

            Node3D moduleInstance = CellData.CollapsedModuleData.PackedScene.Instantiate<Node3D>();
            _parent.AddChild(moduleInstance);

            Quaternion rotation = Rotations.QuaternionByRotation[CellData.CollapsedModuleData.Rotation];
            moduleInstance.SetQuaternion(rotation);
            moduleInstance.SetPosition(position);

            string sceneName =
                System.IO.Path.GetFileNameWithoutExtension(CellData.CollapsedModuleData.PackedScene.ResourcePath);
            moduleInstance.Name = $"Module_{sceneName}{Position}";

            return moduleInstance;
        }

        private ModuleData GetWeightedRandomModule()
        {
            int randomWeight = GD.RandRange(0, CellData.TotalWeight + 1);
            foreach (ModuleData possibleModule in CellData.PossibleModules)
            {
                randomWeight -= possibleModule.Frequency;
                if (randomWeight <= 0)
                {
                    return possibleModule;
                }
            }

            if (!HasPossibilities)
            {
                return null;
            }

            return CellData.PossibleModules[0];
        }

        public bool Propagate(Direction direction, ModuleData collapsedModuleData)
        {
            Queue<ModuleData> impossibleModules = new();

            foreach (ModuleData possibleModule in CellData.PossibleModules)
            {
                if (!ModuleConnectionChecker.CanConnect(collapsedModuleData, possibleModule, direction))
                {
                    bool x = ModuleConnectionChecker.CanConnect(collapsedModuleData, possibleModule, direction);
                    impossibleModules.Enqueue(possibleModule);
                }
            }

            bool changed = impossibleModules.Count != 0;
            foreach (ModuleData impossibleModule in impossibleModules)
            {
                RemoveImpossibleModule(impossibleModule);
            }

            if (!HasPossibilities)
            {
                CellData.IsErroneus = true;
            }

            if (changed)
            {
                CellPropagatedEvent?.Invoke(this);
            }

            return changed;
        }

        private void RemoveImpossibleModule(ModuleData impossibleModule)
        {
            CellData.PossibleModules.Remove(impossibleModule);
            CellData.TotalWeight -= impossibleModule.Frequency;
            CellData.SumOfLogWeight -= Mathf.Log(impossibleModule.Frequency);
        }

        public float GetEntropy()
        {
            float entropy = Mathf.Log(CellData.TotalWeight) - CellData.SumOfLogWeight / CellData.TotalWeight +
                            _entropyNoise;
            if (float.IsNaN(entropy))
            {
                return 0;
            }

            return entropy;
        }

        public void SetWeightData(int totalWeight, float logWeight, float noise)
        {
            CellData.TotalWeight = totalWeight;
            CellData.SumOfLogWeight = logWeight;
            _entropyNoise = noise;
        }
    }
}