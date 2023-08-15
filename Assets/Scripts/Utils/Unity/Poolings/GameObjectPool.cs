using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

namespace Utils.Unity.Poolings
{
    [Serializable]
    public struct GameObjectPoolDescriptor
    {
        [AllowNesting]
        [Required]
        [DisableIf(nameof(IsPlaying))]
        public GameObject Prefab;

        [AllowNesting]
        [Required("Parent is required! When it's null this will spawn GameObjects on Root!")]
        [DisableIf(nameof(IsPlaying))]
        public Transform Parent;

        [AllowNesting]
        [MinValue(1)]
        [DisableIf(nameof(IsPlaying))]
        public int Capacity;

        private static bool IsPlaying()
        {
            return Application.isPlaying;
        }
    }

    public class GameObjectPool : IDisposable
    {
        public int Capacity { get; private set; }
        public int RemainingCount { get; private set; }

        private readonly Queue<GameObject> _Queue;
        private GameObjectPoolDescriptor _Descriptor;

        public GameObjectPool(GameObjectPoolDescriptor descriptor)
        {
            _Descriptor = descriptor;
            _Descriptor.Capacity = Capacity = Mathf.Max(1, _Descriptor.Capacity);

            _Queue = new(_Descriptor.Capacity);
            for (int i = 0; i<_Descriptor.Capacity; i++)
            {
                var obj = UnityEngine.Object.Instantiate(_Descriptor.Prefab, _Descriptor.Parent);
                _Queue.Enqueue(obj);
                obj.SetActive(false);
            }
            RemainingCount = _Descriptor.Capacity;
        }

        public bool TryAllocate(out GameObject obj)
        {
            if (_Queue.TryDequeue(out obj))
            {
                RemainingCount--;
                obj.SetActive(true);
                return true;
            }

            obj = null;
            return false;
        }

        public void Deallocate(GameObject obj)
        {
            if (RemainingCount >= _Descriptor.Capacity)
            {
                throw new InvalidOperationException($"Pool has exceed it's Capacity!");
            }

            RemainingCount++;

            obj.SetActive(false);
            obj.transform.SetParent(_Descriptor.Parent);
            _Queue.Enqueue(obj);
        }

        public void Dispose()
        {
            while(_Queue.TryDequeue(out var obj))
            {
                UnityEngine.Object.Destroy(obj);
            }
            _Queue.Clear();
        }
    }

    public class GameObjectPool<C> : IDisposable where C : Component
    {
        public int Capacity { get; private set; }
        public int RemainingCount { get; private set; }

        private readonly Queue<C> _Queue;
        private GameObjectPoolDescriptor _Descriptor;

        public GameObjectPool(GameObjectPoolDescriptor descriptor)
        {
            _Descriptor = descriptor;
            _Descriptor.Capacity = Capacity = Mathf.Max(1, _Descriptor.Capacity);

            _Queue = new(_Descriptor.Capacity);
            for (int i = 0; i < _Descriptor.Capacity; i++)
            {
                var obj = UnityEngine.Object.Instantiate(_Descriptor.Prefab, _Descriptor.Parent);
                _Queue.Enqueue(obj.GetComponent<C>());
                obj.SetActive(false);
            }
            RemainingCount = _Descriptor.Capacity;
        }

        public bool TryAllocate(out C comp)
        {
            if (_Queue.TryDequeue(out comp))
            {
                RemainingCount--;
                comp.gameObject.SetActive(true);
                return true;
            }

            comp = null;
            return false;
        }

        public void Deallocate(C comp)
        {
            if (RemainingCount >= _Descriptor.Capacity)
            {
                throw new InvalidOperationException($"Pool has exceed it's Capacity!");
            }

            RemainingCount++;
            comp.gameObject.SetActive(false);
            comp.transform.SetParent(_Descriptor.Parent);
            _Queue.Enqueue(comp);
        }

        public void Dispose()
        {
            while (_Queue.TryDequeue(out var obj))
            {
                UnityEngine.Object.Destroy(obj);
            }
            _Queue.Clear();
        }
    }
}
