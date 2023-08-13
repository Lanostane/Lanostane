using System.Collections.Generic;
using UnityEngine.Scripting;
using ZeroFormatter;
using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
using ID = ZeroFormatter.IndexAttribute;

namespace Lanostane.Models
{
    [Preserve]
    [ZeroFormattable]
    public class LST_DefaultMotion
    {
        [ID(0)] public virtual float Degree { get; set; } = 0.0f;
        [ID(1)] public virtual float Radius { get; set; } = 0.0f;
        [ID(2)] public virtual float Rotation { get; set; } = 0.0f;
        [ID(3)] public virtual float Height { get; set; } = -20.0f;
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_RotationMotion
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float Duration { get; set; }
        [ID(2)] public virtual float DeltaRotation { get; set; }
        [ID(3)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_XYLinearMotion
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float Duration { get; set; }
        [ID(2)] public virtual float NewDegree { get; set; }
        [ID(3)] public virtual float NewRadius { get; set; }
        [ID(4)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_XYCirclerMotion
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float Duration { get; set; }
        [ID(2)] public virtual float DeltaDegree { get; set; }
        [ID(3)] public virtual float DeltaRadius { get; set; }
        [ID(4)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_HeightMotion
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float Duration { get; set; }
        [ID(2)] public virtual float DeltaHeight { get; set; }
        [ID(3)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_BPMChange
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float BPM { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_ScrollChange
    {
        [ID(0)] public virtual ushort Group { get; set; } = 0;
        [ID(1)] public virtual float Timing { get; set; }
        [ID(2)] public virtual float Speed { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_SubCameraAlphaChange
    {
        [ID(0)] public virtual float Timing { get; set; }
        [ID(1)] public virtual float Duration { get; set; }
        [ID(2)] public virtual LST_Ease Ease { get; set; }
    }

    [Preserve]
    [ZeroFormattable]
    public class LST_SubCameraMotions
    {
        [ID(0)] public virtual LST_CameraIndex CameraIndex { get; protected set; }
        [ID(1)] public virtual List<LST_XYLinearMotion> LinearMos { get; protected set; } = new();
        [ID(2)] public virtual List<LST_XYCirclerMotion> CirclerMos { get; protected set; } = new();
        [ID(3)] public virtual List<LST_HeightMotion> HeightMos { get; protected set; } = new();
        [ID(4)] public virtual List<LST_SubCameraAlphaChange>  AlphaChanges { get; protected set; } = new();
    }
}
