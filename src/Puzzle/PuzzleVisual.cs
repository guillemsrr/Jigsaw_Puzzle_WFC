// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;

namespace GodotTest.Puzzle
{
    public partial class PuzzleVisual : MeshInstance3D
    {
        public void UpdateUv(PuzzlePiece data)
        {
            var baseMat = GetActiveMaterial(0) as StandardMaterial3D;
            if (baseMat == null)
            {
                GD.PushWarning("PuzzleVisual3D: Material is not a StandardMaterial3D.");
                return;
            }

            StandardMaterial3D matInstance = (StandardMaterial3D) baseMat.Duplicate(true);
            //var Uv1Offset = matInstance.Uv1Offset;
            //var Uv1Scale = matInstance.Uv1Scale;

            Vector2 offset = data.UVRegionOrigin;
            Vector2 scale = data.UVRegionSize;

            matInstance.Uv1Offset = new Vector3(offset.X, offset.Y, 0);
            matInstance.Uv1Scale = new Vector3(scale.X, scale.Y, 1);
            //matInstance.AlbedoTexture = data.Texture;

            SetMaterialOverride(matInstance);
        }
    }
}