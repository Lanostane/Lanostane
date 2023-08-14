using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class Coroutines
    {
        private static Coroutines_Impl s_Instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Setup()
        {
            var dispatcher = new GameObject($"[{nameof(Coroutine)}]");
            s_Instance = dispatcher.AddComponent<Coroutines_Impl>();
            UnityEngine.Object.DontDestroyOnLoad(dispatcher);
        }

        public static Coroutine Start(IEnumerator routine)
        {
            return s_Instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine routine)
        {
            s_Instance.StopCoroutine(routine);
        }
    }
}
