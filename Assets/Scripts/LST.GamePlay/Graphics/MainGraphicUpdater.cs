using UnityEngine;

namespace LST.GamePlay.Graphics
{
    public sealed class MainGraphicUpdater : MonoBehaviour
    {
        private readonly Vector3 _ClockwiseRot = new(0.0f, 0.0f, -1.0f);

        public Color JudgeColorStart;
        public Color JudgeColorEnd;
        public Transform Core;
        public Transform JudgeLine;
        public Transform Background;
        public SpriteRenderer JudgeLineGlow;

        void Awake()
        {
            BPM.BPMChanged += BPMChanged;
        }

        void OnDestroy()
        {
            BPM.BPMChanged -= BPMChanged;
        }

        private void BPMChanged(float bpm)
        {
            _BPM = bpm;
            _RotPerSec = _BPM / 8.0f;
            _ColorTime = _BPM / 30.0f;
        }

        void FixedUpdate()
        {
            Core.Rotate(0.1f * _RotPerSec * Time.fixedDeltaTime * -_ClockwiseRot);
            JudgeLine.Rotate(0.5f * _RotPerSec * Time.fixedDeltaTime * -_ClockwiseRot);
            Background.Rotate(0.5f * _RotPerSec * Time.fixedDeltaTime * _ClockwiseRot);

            JudgeLineGlow.color = Color.Lerp(JudgeColorStart, JudgeColorEnd, Mathf.PingPong(Time.time * _ColorTime, 1.0f));
        }

        private float _BPM = 100.0f;
        private float _RotPerSec = 100.0f / 8.0f;
        private float _ColorTime = 100.0f / 30f;
    }
}