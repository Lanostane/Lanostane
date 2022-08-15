using System;
using UnityEngine;

namespace Utils.Maths
{
    public struct MathfE
    {
        public static float AbsDeltaAngle(float angle1, float angle2)
        {
            return Math.Abs(Mathf.DeltaAngle(angle1, angle2));
        }

        public static float AbsDelta(float a, float b)
        {
            return Math.Abs(a - b);
        }

        public static float AbsAngle(float angle)
        {
            return Mathf.Repeat(angle, 360.0f);
        }

        public static bool AbsApprox(float a, float b, float tolerance)
        {
            return Math.Abs(a - b) <= tolerance;
        }

        public static bool ApproxAngle(float angle1, float angle2, float tolerance)
        {
            return Math.Abs(Mathf.DeltaAngle(angle1, angle2)) <= tolerance;
        }
    }
}
