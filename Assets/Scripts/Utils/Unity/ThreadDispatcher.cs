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
        private static ThreadDispatcher_Impl _Instance;
        private static readonly ConcurrentQueue<Action> _PreSetupQueue = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Setup()
        {
            var dispatcher = new GameObject($"[{nameof(ThreadDispatcher)}]");
            _Instance = dispatcher.AddComponent<ThreadDispatcher_Impl>();
            UnityEngine.Object.DontDestroyOnLoad(dispatcher);

            while(_PreSetupQueue.TryDequeue(out var action))
            {
                _Instance.Queue.Enqueue(action);
            }
        }

        public static void Dispatch(Action action)
        {
            if (_Instance == null)
            {
                _PreSetupQueue.Enqueue(action);
            }
            else
            {
                _Instance.Queue.Enqueue(action);
            }
        }
    }
}
