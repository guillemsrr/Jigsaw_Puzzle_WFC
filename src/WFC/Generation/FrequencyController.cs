using System;
using System.Collections.Generic;
using GodotTest.WFC.Adjacency;
using GodotTest.WFC.Generation.Cells;
using GodotTest.WFC.Modules;

namespace GodotTest.WFC.Generation
{
    public class FrequencyController
    {
        private const int HIGH_FREQUENCY = 50;
        private readonly Random random = new Random();

        public void SetOneRandomElementHighFrequency(ModuleData[] AdjacencyDatas)
        {
            int randomIndex = random.Next(0, AdjacencyDatas.Length);
            SetSpecificElementHighFrequency(AdjacencyDatas, randomIndex);
        }

        public void SetSpecificElementRandomFrequency(ModuleData[] AdjacencyDatas, int index)
        {
            AdjacencyDatas[index].Frequency = random.Next(0, HIGH_FREQUENCY);
        }

        public void SetSpecificElementHighFrequency(ModuleData[] AdjacencyDatas, int index)
        {
            AdjacencyDatas[index].Frequency = HIGH_FREQUENCY;
        }

        public void SetRandomFrequencies(ModuleData[] AdjacencyDatas)
        {
            foreach (ModuleData AdjacencyData in AdjacencyDatas)
            {
                AdjacencyData.Frequency = random.Next(0, 10);
            }
        }

        public void CalculateInitialWeight(ICollection<CellController> cells)
        {
            foreach (CellController cell in cells)
            {
                CalculateInitialWeight(cell);
            }
        }

        private void CalculateInitialWeight(CellController cellController)
        {
            int totalWeight = 0;
            double logWeight = 0;

            foreach (ModuleData possibleModule in cellController.Possibilities)
            {
                totalWeight += possibleModule.Frequency;

                logWeight += Math.Log(possibleModule.Frequency);
            }

            double noise = random.NextDouble() / 100.0;

            cellController.SetWeightData(totalWeight, (float) logWeight, (float) noise);
        }
    }
}