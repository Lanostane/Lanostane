using Charts;
using GamePlay.Judge.Handles;
using GamePlay.Motions;
using UnityEngine;
using Utils;

namespace GamePlay.Judge.Inputs
{
    public enum InputEvent : byte
    {
        None,
        PointerDown,
        PointerHold,
        StartDrag,
        Drag,
        Flick,
        EndDrag,
        PointerUp,

        AutoPlay = byte.MaxValue
    }

    public sealed class InputHandle
    {
        public const float FlickThreshold = 0.3f;

        public readonly int ID;
        public InputEvent EventType;
        public float Angle;
        public float GameAngle => Angle + MotionManager.Instance.CurrentRotation;
        public float Timing;

        public bool Holding;

        public bool Dragging;
        public bool InValidPosition;

        public Vector3 PreviousFlickPosition;
        public float FlickAmount;
        public float FlickAngle;
        public float GameFlickAngle => FlickAngle + MotionManager.Instance.CurrentRotation;
        public LST_FlickDir LastFlickDir;
        public bool HasHandledFlick;

        public InputHandle(int id)
        {
            ID = id;
            EventType = InputEvent.None;
        }

        public void Reset()
        {
            EventType = InputEvent.None;
            Angle = 0.0f;
            Timing = 0.0f;

            Holding = false;
            Dragging = false;
            ResetFlick();
        }

        public void ResetFlick()
        {
            PreviousFlickPosition = Vector3.zero;
            FlickAmount = 0.0f;
            FlickAngle = 0.0f;
            LastFlickDir = LST_FlickDir.In;
            HasHandledFlick = false;
        }

        public void SetDegreeByWorldPosition(Vector3 position)
        {
            var deg = (Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg) + 90.0f;
            Angle = deg;
        }

        public bool TryUpdateFlick(Vector3 position)
        {
            if ((position - PreviousFlickPosition).sqrMagnitude < (FlickThreshold * FlickThreshold))
                return false;

            UpdateFlick(position);
            return true;
        }

        private void UpdateFlick(Vector3 position)
        {
            var delta = PreviousFlickPosition - position;
            FlickAmount = delta.magnitude;
            FlickAngle = (Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg) + 90.0f;

            Debug.DrawLine(PreviousFlickPosition, position, Color.blue, 1.0f);
            PreviousFlickPosition = position;
        }
    }
}
