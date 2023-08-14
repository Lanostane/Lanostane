using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public static class ThreadDispatcher
    {
        private static ThreadDispatcher_Impl s_Instance;
        private static readonly ConcurrentQueue<Action> s_PreSetupQueue = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Setup()
        {
            var dispatcher = new GameObject($"[{nameof(ThreadDispatcher)}]");
            s_Instance = dispatcher.AddComponent<ThreadDispatcher_Impl>();
            UnityEngine.Object.DontDestroyOnLoad(dispatcher);

            while(s_PreSetupQueue.TryDequeue(out var action))
            {
                s_Instance.Queue.Enqueue(action);
            }
        }

        public static void Dispatch(Action action)
        {
            if (s_Instance == null)
            {
                s_PreSetupQueue.Enqueue(action);
            }
            else
            {
                s_Instance.Queue.Enqueue(action);
            }
        }
    }
}
