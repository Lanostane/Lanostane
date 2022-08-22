using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Unity
{
    public sealed class ThreadDispatcher_Impl : MonoBehaviour
    {
        public readonly ConcurrentQueue<Action> Queue = new();

        void Update()
        {
            if (Queue.Count > 0)
            {
                while (Queue.TryDequeue(out var action))
                {
                    action?.Invoke();
                }
            }
        }
    }
}
