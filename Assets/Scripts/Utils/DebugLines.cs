using GamePlay;
using GamePlay.Motions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Maths;

namespace Utils
{
    public static class DebugLines
    {
        [Conditional("UNITY_EDITOR")]
        public static void DrawToBorder(float worlddegree, Color color, float time = 1.0f)
        {
            var x = FastTrig.Sin(worlddegree);
            var y = FastTrig.Cos(worlddegree);
            var start = new Vector3(x, y, 0.0f) * GameConst.SpaceEnd;
            var end = new Vector3(x, y, 0.0f) * (GameConst.SpaceEnd + 1.5f);
            UnityEngine.Debug.DrawLine(start, end, color, time);
        }
    }
}
