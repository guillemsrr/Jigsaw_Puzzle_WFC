// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;

namespace GodotTest.Puzzle._3d;

public partial class PuzzleVisual : MeshInstance3D
{
	public override void _Ready()
	{
		base._Ready();

		if (Mesh is not PrimitiveMesh primitiveMesh)
		{
			GD.PushError("Mesh is not a PrimitiveMesh");
			return;
		}

		ArrayMesh arrayMesh = new ArrayMesh();
		arrayMesh.AddSurfaceFromArrays(
			Mesh.PrimitiveType.Triangles,
			primitiveMesh.GetMeshArrays()
		);

		Mesh = arrayMesh;

		var arrays = arrayMesh.SurfaceGetArrays(0);
		Vector2[] uvs = arrays[(int) ArrayMesh.ArrayType.TexUV].AsVector2Array();

		foreach (var uv in uvs)
		{
			//GD.Print("UV: ", uv);
		}
	}

	public void UpdateUV(PuzzlePiece data)
	{
		var baseMat = GetActiveMaterial(0) as StandardMaterial3D;
		if (baseMat == null)
		{
			GD.PushWarning("PuzzleVisual3D: Material is not a StandardMaterial3D.");
			return;
		}

		StandardMaterial3D matInstance = (StandardMaterial3D) baseMat.Duplicate(true);
		var Uv1Offset = matInstance.Uv1Offset;
		var Uv1Scale = matInstance.Uv1Scale;

		Vector2 offset = data.UVRegionOrigin;
		Vector2 scale = data.UVRegionSize;

		matInstance.Uv1Offset = new Vector3(offset.X, offset.Y, 0);
		matInstance.Uv1Scale = new Vector3(scale.X, scale.Y, 1);
		//matInstance.AlbedoTexture = data.Texture;

		SetMaterialOverride(matInstance);
	}
}