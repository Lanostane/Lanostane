using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Maths;
using Utils.Unity.Impl;

namespace Utils.Unity
{
    public static class DebugLines
    {
        [Conditional("UNITY_EDITOR")]
        public static void DrawToBorder(float worlddegree, Color color, float time = 1.0f)
        {
            var x = FastTrig.DegSin(worlddegree);
            var y = -FastTrig.DegCos(worlddegree);
            var start = new Vector3(x, y, 0.0f) * 10.0f;
            var end = new Vector3(x, y, 0.0f) * 11.5f;

            DrawLine(start, end, color, time);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 pos1, Vector3 pos2, Color color, float time = 1.0f)
        {
            time = Mathf.Clamp(time, 0.01f, 10.0f);
            color.a = Mathf.Min(color.a * 0.65f, 0.65f);
            if (DebugLines_Impl.Instance != null)
            {
                DebugLines_Impl.Instance.DrawLine(pos1, pos2, color, time);
            }
        }
    }
}
