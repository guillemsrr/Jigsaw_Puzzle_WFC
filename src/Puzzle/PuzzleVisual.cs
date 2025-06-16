// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;

namespace GodotTest.Puzzle
{
    public partial class PuzzleVisual : MeshInstance3D
    {
        public ShaderMaterial GetShader()
        {
            var baseMat = GetActiveMaterial(0) as ShaderMaterial;
            if (baseMat == null)
            {
                GD.PushWarning("PuzzleVisual3D: Material is not a ShaderMaterial.");
                return null;
            }

            ShaderMaterial shaderMat = (ShaderMaterial) baseMat.Duplicate(true);
            return shaderMat;
        }
    }
}