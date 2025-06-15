using System.Collections.Generic;
using Godot;

namespace GodotTest.WFC.Utilities
{
    public enum Rotation
    {
        Rot0,
        Rot90,
        Rot180,
        Rot270,
    }

    public class Rotations
    {
        public static Dictionary<Rotation, Quaternion> QuaternionByRotation = new Dictionary<Rotation, Quaternion>
        {
            [Rotation.Rot0] = Quaternion.Identity,
            [Rotation.Rot90] = Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(-90), 0)),
            [Rotation.Rot180] = Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(180), 0)),
            [Rotation.Rot270] = Quaternion.FromEuler(new Vector3(0, Mathf.DegToRad(90), 0))
        };
    }
}