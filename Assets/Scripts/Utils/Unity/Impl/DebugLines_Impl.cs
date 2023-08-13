using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Unity.Impl
{
    internal class DebugLines_Impl : MonoBehaviour
    {
        public bool Enabled;
        public GameObject LineRendererPrefab;
        public int PoolCount = 200;

        [ReadOnly]
        [SerializeField]
        private int _RemainingInPool = 0;

        private Queue<LineRenderer> _AllocatableLines;
        private Vector3[] _Buffer;

        internal static DebugLines_Impl Instance { get; private set; }

        void Awake()
        {
            Instance = this;

            _AllocatableLines = new(PoolCount);
            _Buffer = new Vector3[2];
            for (int i = 0; i<PoolCount; i++)
            {
                var obj = Instantiate(LineRendererPrefab, transform);
                obj.SetActive(false);
                _AllocatableLines.Enqueue(obj.GetComponent<LineRenderer>());
            }
            _RemainingInPool = _AllocatableLines.Count;
        }

        public void DrawLine(Vector3 pos1, Vector3 pos2, Color color, float time)
        {
            if (!Enabled)
                return;

            if (_AllocatableLines.TryDequeue(out var line))
            {
                StartCoroutine(SetLineLife(line, time));

                line.startColor = color;
                line.endColor = color;

                _Buffer[0] = pos1;
                _Buffer[1] = pos2;
                line.SetPositions(_Buffer);

                _RemainingInPool = _AllocatableLines.Count;
            }
        }

        IEnumerator SetLineLife(LineRenderer renderer, float time)
        {
            renderer.gameObject.SetActive(true);

            yield return new WaitForSecondsRealtime(time);

            renderer.gameObject.SetActive(false);
            _AllocatableLines.Enqueue(renderer);
            _RemainingInPool = _AllocatableLines.Count;
        }

        void OnDestroy()
        {
            Instance = null;
            _AllocatableLines.Clear();
            _AllocatableLines = null;
            _Buffer = null;
        }
    }
}
