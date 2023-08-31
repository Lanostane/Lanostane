using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class EditorLog
    {
        [Conditional("UNITY_EDITOR")]
        public static void Info(object message, UnityEngine.Object context = null)
        {
            if (context == null) UnityEngine.Debug.Log(message);
            else UnityEngine.Debug.Log(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Warn(object message, UnityEngine.Object context = null)
        {
            if (context == null) UnityEngine.Debug.LogWarning(message);
            else UnityEngine.Debug.LogWarning(message, context);
        }

        [Conditional("UNITY_EDITOR")]
        public static void Error(object message, UnityEngine.Object context = null)
        {
            if (context == null) UnityEngine.Debug.LogError(message);
            else UnityEngine.Debug.LogError(message, context);
        }
    }
}
