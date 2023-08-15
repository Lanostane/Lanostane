using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using Utils.Unity.Poolings;

namespace Utils.Unity.Impl
{
    internal class DebugLines_Impl : MonoBehaviour
    {
        public bool Enabled;
        public GameObjectPoolDescriptor PoolOptions;

        [ReadOnly]
        [SerializeField]
        [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Yeah")]
        private int _RemainingInPool = 0;

        private Vector3[] _Buffer;
        private GameObjectPool<LineRenderer> _Pool;

        internal static DebugLines_Impl Instance { get; private set; }

        void Awake()
        {
            Instance = this;
            _Buffer = new Vector3[2];
            _Pool = new(PoolOptions);
            _RemainingInPool = _Pool.RemainingCount;
        }

        public void DrawLine(Vector3 pos1, Vector3 pos2, Color color, float time)
        {
            if (!Enabled)
                return;

            if (_Pool.TryAllocate(out var line))
            {
                StartCoroutine(SetLineLife(line, time));

                line.startColor = color;
                line.endColor = color;

                _Buffer[0] = pos1;
                _Buffer[1] = pos2;
                line.SetPositions(_Buffer);

                _RemainingInPool = _Pool.RemainingCount;
            }
        }

        IEnumerator SetLineLife(LineRenderer renderer, float time)
        {
            renderer.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(time);

            renderer.gameObject.SetActive(false);
            _Pool.Deallocate(renderer);
            _RemainingInPool = _Pool.RemainingCount;
        }

        void OnDestroy()
        {
            Instance = null;
            _Pool.Dispose();
            _Pool = null;
            _Buffer = null;
        }
    }
}
