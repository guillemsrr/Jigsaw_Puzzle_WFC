// Copyright (c) Guillem Serra. All Rights Reserved.

using System.Collections.Generic;

namespace GodotTest.WFC.Adjacency
{
    public static class AdjacencyRules
    {
        private static readonly Dictionary<AdjacencyType, List<AdjacencyType>> Rules = new()
        {
            {AdjacencyType.Flat, [AdjacencyType.Flat]},
            {AdjacencyType.Slot, [AdjacencyType.Tab]},
            {AdjacencyType.Tab, [AdjacencyType.Slot]}
        };

        public static bool CanBeAdjacent(AdjacencyType type1, AdjacencyType type2)
        {
            return Rules[type1].Contains(type2) || Rules[type2].Contains(type1);
        }
    }
}