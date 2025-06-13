// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;
using GodotTest.Puzzle._3d;

namespace GodotTest.Drag;

public partial class Draggable : Node3D
{
	[Export] public Node3D Parent;
	[Export] public StaticBody3D Target;
	private Camera3D _camera;
	private bool _dragging;

	public override void _Ready()
	{
		_camera = GetViewport().GetCamera3D();
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (Target == null) return;

		if (@event is InputEventMouseButton mb && mb.ButtonIndex == MouseButton.Left)
		{
			if (mb.Pressed)
			{
				// Start raycast
				Vector3 from = _camera.ProjectRayOrigin(mb.Position);
				Vector3 to = from + _camera.ProjectRayNormal(mb.Position) * 1000f;

				var space = GetWorld3D().DirectSpaceState;
				var query = PhysicsRayQueryParameters3D.Create(from, to);
				query.CollideWithAreas = false;
				query.CollideWithBodies = true;

				var result = space.IntersectRay(query);

				if (result.TryGetValue("collider", out var collider) &&
					collider.Obj == Target)
				{
					_dragging = true;
				}
			}
			else
			{
				_dragging = false;
			}
		}
		else if (@event is InputEventMouseMotion mm && _dragging)
		{
			Vector3 from = _camera.ProjectRayOrigin(mm.Position);
			Vector3 dir = _camera.ProjectRayNormal(mm.Position);

			// Intersect with horizontal plane (Y=0)
			float t = -from.Y / dir.Y;
			Vector3 intersect = from + dir * t;

			Parent.SetGlobalPosition(intersect);;
		}
	}
}
