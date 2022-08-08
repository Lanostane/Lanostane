using System.Collections.Generic;
using Utils;

namespace Charts
{
    public class LST_Chart
    {
        public float SongLength;
        public float StartBPM;
        public LST_DefaultMotion Default = new();
        public readonly List<LST_Tap> TapNotes = new();
        public readonly List<LST_Catch> CatchNotes = new();
        public readonly List<LST_Flick> FlickNotes = new();
        public readonly List<LST_Hold> HoldNotes = new();

        public readonly List<LST_RotationMotion> RotationMos = new();
        public readonly List<LST_XYLinearMotion> LinearMos = new();
        public readonly List<LST_XYCirclerMotion> CirclerMos = new();
        public readonly List<LST_HeightMotion> HeightMos = new();

        public readonly List<LST_BPMChange> BPMs = new();
        public readonly List<LST_ScrollChange> Scrolls = new();
    }

    public enum LST_Ease
    {
        Linear = 0,

        InQuart = 1,
        OutQuart = 2,
        InOutQuart = 3,

        InCubic = 4,
        OutCubic = 5,
        InOutCubic = 6,

        InExpo = 7,
        OutExpo = 8,
        InOutExpo = 9,

        InSine = 10,
        OutSine = 11,
        InOutSine = 12
    }

    public static class EaseExtension
    {
        public static float EvalClamped(this LST_Ease ease, float t)
        {
            if (t <= 0.0f)
                return 0.0f;
            else if (t >= 1.0f)
                return 1.0f;


            return ease switch
            {
                LST_Ease.Linear => t,
                LST_Ease.InQuart => Ease.Quartic.In(t),
                LST_Ease.OutQuart => Ease.Quartic.Out(t),
                LST_Ease.InOutQuart => Ease.Quartic.InOut(t),

                LST_Ease.InCubic => Ease.Cubic.In(t),
                LST_Ease.OutCubic => Ease.Cubic.Out(t),
                LST_Ease.InOutCubic => Ease.Cubic.InOut(t),

                LST_Ease.InExpo => Ease.Exponential.In(t),
                LST_Ease.OutExpo => Ease.Exponential.Out(t),
                LST_Ease.InOutExpo => Ease.Exponential.InOut(t),

                LST_Ease.InSine => Ease.Sinusoidal.In(t),
                LST_Ease.OutSine => Ease.Sinusoidal.Out(t),
                LST_Ease.InOutSine => Ease.Sinusoidal.InOut(t),
                _ => t,
            };
        }
    }
}
