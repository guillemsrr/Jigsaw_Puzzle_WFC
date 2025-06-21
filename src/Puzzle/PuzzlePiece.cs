// Copyright (c) Guillem Serra. All Rights Reserved.

using Godot;
using GodotTest.Drag;
using GodotTest.WFC.Adjacency;

namespace GodotTest.Puzzle
{
    public partial class PuzzlePiece : Node3D
    {
        [Export] public PuzzleVisual PuzzleVisual;
        [Export] public Draggable Draggable;
        [Export] public AdjacencyData AdjacencyData;
        [Export] private float RotationSpeed = 0.3f;

        private float _targetRotation = 0.0f;

        private Tween _rotationTween;
        public Vector3 PuzzlePosition { get; private set; }
        public Quaternion PuzzleRotation { get; private set; }

        public void SetCurrentAsPuzzleSolution()
        {
            PuzzlePosition = GetGlobalPosition();
            PuzzleRotation = GetQuaternion();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            InputEventMouseButton mb = @event as InputEventMouseButton;
            if (mb == null)
            {
                return;
            }

            if (!Draggable.IsMouseOn(mb))
            {
                return;
            }

            if (mb.ButtonIndex == MouseButton.Right && mb.Pressed)
            {
                Rotate();
            }
        }

        private void Rotate()
        {
            float currentYaw = Mathf.PosMod(Rotation.Y, Mathf.Tau);
            float newTarget = Mathf.PosMod(_targetRotation + Mathf.Pi / 2, Mathf.Tau);

            if (newTarget <= currentYaw)
            {
                newTarget += Mathf.Tau;
            }

            _targetRotation = newTarget;

            if (_rotationTween != null)
            {
                _rotationTween.Kill();
            }

            _rotationTween = CreateTween();

            _rotationTween.TweenMethod(
                    Callable.From<float>(angle =>
                    {
                        var rot = Rotation;
                        rot.Y = Mathf.PosMod(angle, Mathf.Tau);
                        Rotation = rot;
                    }),
                    currentYaw,
                    _targetRotation,
                    RotationSpeed
                )
                .SetEase(Tween.EaseType.InOut)
                .SetTrans(Tween.TransitionType.Quad);
        }
    }
}