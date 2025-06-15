using System.Collections.Generic;
using GodotTest.WFC.Adjacency;
using GodotTest.WFC.Utilities;

namespace GodotTest.WFC.Modules
{
    public class ModuleRotationChecker
    {
        private readonly Rotation[] _rotations = { Rotation.Rot90, Rotation.Rot180, Rotation.Rot270 };
        private AdjacencyData _AdjacencyData;
        public List<Rotation> GetAllDifferentRotations(AdjacencyData AdjacencyData)
        {
            _AdjacencyData = AdjacencyData;
            _AdjacencyData.InitializeDirectionsDictionary();
            
            List<Rotation> rotations = new List<Rotation>();
            rotations.Add(Rotation.Rot0);

            foreach (Rotation rotation in _rotations)
            {
                if (!DuplicatedRotation(rotation, rotations))
                {
                    rotations.Add(rotation);
                }
            }

            return rotations;
        }

        private bool DuplicatedRotation(Rotation newRotation, List<Rotation> rotations)
        {
            foreach (Rotation differentRotation in rotations)
            {
                if (!AreAnyOfModuleFacesDifferent(differentRotation, newRotation))
                {
                    return true;
                }
            }

            return false;
        }

        private bool AreAnyOfModuleFacesDifferent(Rotation differentRotation, Rotation newRotation)
        {
            foreach (Direction horizontalDirection in Directions.HorizontalDirections)
            {
                AdjacencyType differentFace = _AdjacencyData.FacesByDirection[Directions.RotateDirection(horizontalDirection, differentRotation)];
                AdjacencyType newFace = _AdjacencyData.FacesByDirection[Directions.RotateDirection(horizontalDirection, newRotation)];

                if (differentFace != newFace) return true;
            }

            return false;
        }
    }
}