using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Lanostane.Charts
{
    public enum LST_CameraIndex : byte
    {
        Main,
        Sub_1,
        Sub_2,
        Sub_3,
        Sub_4,
        Sub_5,
        Sub_6,
        Sub_7,
        Sub_8,
        Sub_9,
        Sub_10,
        Sub_11,
        Sub_12,
        Sub_13,
        Sub_14,
        Sub_15,
        Sub_16
    }

    [Preserve]
    public class LST_DefaultMotion
    {
        public float Degree = 0.0f;
        public float Radius = 0.0f;
        public float Rotation = 0.0f;
        public float Height = -20.0f;
    }

    [Preserve]
    public class LST_RotationMotion
    {
        public float Timing;
        public float Duration;
        public float DeltaRotation;
        public LST_Ease Ease;
    }

    [Preserve]
    public class LST_XYLinearMotion
    {
        public float Timing;
        public float Duration;
        public float NewDegree;
        public float NewRadius;
        public LST_Ease Ease;
    }

    [Preserve]
    public class LST_XYCirclerMotion
    {
        public float Timing;
        public float Duration;
        public float DeltaDegree;
        public float DeltaRadius;
        public LST_Ease Ease;
    }

    [Preserve]
    public class LST_HeightMotion
    {
        public float Timing;
        public float Duration;
        public float DeltaHeight;
        public LST_Ease Ease;
    }

    [Preserve]
    public class LST_BPMChange
    {
        public float Timing;
        public float BPM;
    }

    [Preserve]
    public class LST_ScrollChange
    {
        public ushort Group = 0;
        public float Timing;
        public float Speed;
    }

    [Preserve]
    public class LST_SubCameraAlphaChange
    {
        public float Timing;
        public float Duration;
        public LST_Ease Ease;
    }

    [Preserve]
    public class LST_SubCameraMotions
    {
        public LST_CameraIndex CameraIndex;
        public readonly List<LST_XYLinearMotion> LinearMos = new();
        public readonly List<LST_XYCirclerMotion> CirclerMos = new();
        public readonly List<LST_HeightMotion> HeightMos = new();
        public readonly List<LST_SubCameraAlphaChange> AlphaChanges = new();
    }
}
