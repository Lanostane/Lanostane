using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.Maths;

namespace Lanostane.Models
{
    public enum LST_Ease : byte
    {
        Linear = 0,
        One = 1,
        Zero = 2,

        InQuadratic,
        OutQuadratic,
        InOutQuadratic,

        InCubic,
        OutCubic,
        InOutCubic,

        InQuartic,
        OutQuartic,
        InOutQuartic,

        InQuintic,
        OutQuintic,
        InOutQuintic,

        InSinusoidal,
        OutSinusoidal,
        InOutSinusoidal,

        InExponential,
        OutExponential,
        InOutExponential,

        InCircular,
        OutCircular,
        InOutCircular,

        InElastic,
        OutElastic,
        InOutElastic,

        InBack,
        OutBack,
        InOutBack,

        InBounce,
        OutBounce,
        InOutBounce
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
                LST_Ease.One => 1.0f,
                LST_Ease.Zero => 0.0f,

                LST_Ease.InQuadratic => Ease.Quadratic.In(t),
                LST_Ease.OutQuadratic => Ease.Quadratic.Out(t),
                LST_Ease.InOutQuadratic => Ease.Quadratic.InOut(t),

                LST_Ease.InCubic => Ease.Cubic.In(t),
                LST_Ease.OutCubic => Ease.Cubic.Out(t),
                LST_Ease.InOutCubic => Ease.Cubic.InOut(t),

                LST_Ease.InQuartic => Ease.Quartic.In(t),
                LST_Ease.OutQuartic => Ease.Quartic.Out(t),
                LST_Ease.InOutQuartic => Ease.Quartic.InOut(t),

                LST_Ease.InQuintic => Ease.Quintic.In(t),
                LST_Ease.OutQuintic => Ease.Quintic.Out(t),
                LST_Ease.InOutQuintic => Ease.Quintic.InOut(t),

                LST_Ease.InSinusoidal => Ease.Sinusoidal.In(t),
                LST_Ease.OutSinusoidal => Ease.Sinusoidal.Out(t),
                LST_Ease.InOutSinusoidal => Ease.Sinusoidal.InOut(t),

                LST_Ease.InExponential => Ease.Exponential.In(t),
                LST_Ease.OutExponential => Ease.Exponential.Out(t),
                LST_Ease.InOutExponential => Ease.Exponential.InOut(t),

                LST_Ease.InCircular => Ease.Circular.In(t),
                LST_Ease.OutCircular => Ease.Circular.Out(t),
                LST_Ease.InOutCircular => Ease.Circular.InOut(t),

                LST_Ease.InElastic => Ease.Elastic.In(t),
                LST_Ease.OutElastic => Ease.Elastic.Out(t),
                LST_Ease.InOutElastic => Ease.Elastic.InOut(t),

                LST_Ease.InBack => Ease.Back.In(t),
                LST_Ease.OutBack => Ease.Back.Out(t),
                LST_Ease.InOutBack => Ease.Back.InOut(t),

                LST_Ease.InBounce => Ease.Bounce.In(t),
                LST_Ease.OutBounce => Ease.Bounce.Out(t),
                LST_Ease.InOutBounce => Ease.Bounce.InOut(t),

                _ => t,
            };
        }
    }
}
