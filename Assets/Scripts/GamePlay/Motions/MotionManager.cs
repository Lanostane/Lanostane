using Charts;
using GamePlay.Motions.Collections;
using System.Collections;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Motions
{
    public interface IMotionManager : IChartUpdater
    {
        float CurrentBPM { get; }
        float CurrentRotation { get; }
        float StartingTheta { get; }
        float StartingRho { get; }
        float StartingHeight { get; }
        Vector3 StartingPosition { get; }
        PolarPoint StartingPolar { get; }
        float StartingRotation { get; }

        IMotionWorker Main { get; }

        void AddMotions(LST_Chart chart);
        void UpdateAbsValue();
        void StartDefaultMotion(float duration);
        bool TryGetBPMByTime(float time, out float bpm);
        void SetDefaultMotion(LST_DefaultMotion defaultMotion);
    }

    internal class MotionManager : MonoBehaviour, IMotionManager
    {
        public static IMotionManager Instance { get; private set; }

        public Transform RotationOrigin;

        public float CurrentBPM { get; private set; }
        public float CurrentRotation => _Main.CurrentRotation;
        public float StartingTheta { get; private set; }
        public float StartingRho { get; private set; }
        public float StartingHeight { get; private set; }
        public Vector3 StartingPosition { get; private set; }
        public PolarPoint StartingPolar { get; private set; }
        public float StartingRotation { get; private set; }

        public IMotionWorker Main => _Main;

        [SerializeField] private MotionWorker _Main;

        private readonly MotionsBpm _BPMS = new();

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        public void AddMotions(LST_Chart chart)
        {
            _Main.Setup(chart);

            foreach (var bpm in chart.BPMs)
            {
                _BPMS.AddBpmChange(bpm);
            }
            _BPMS.OnUpdateMotion += Update_BPM;
        }

        public void UpdateAbsValue()
        {
            _Main.SortData();
            _BPMS.UpdateMotionAbsData();
        }

        public void StartDefaultMotion(float duration)
        {
            _Main.StartDefaultMotion(duration);
        }

        public void UpdateChart(float chartTime)
        {
            _Main.UpdateChart(chartTime);
            _BPMS.UpdateChartTime(chartTime);
        }

        private void Update_BPM(BpmMotion m, float p)
        {
            if (m.Bpm != CurrentBPM)
            {
                BPM.Invoke_BPMChange(m.Bpm);
                CurrentBPM = m.Bpm;
            }
        }

        public void CleanUp()
        {
            _Main.CleanUp();
            _BPMS.Clear();
        }

        public bool TryGetBPMByTime(float time, out float bpm)
        {
            if (_BPMS.TryGetLastMotion(time, out var motion))
            {
                bpm = motion.Bpm;
                return true;
            }
            else
            {
                bpm = 0.0f;
                return false;
            }
        }

        public void SetDefaultMotion(LST_DefaultMotion defaultMotion)
        {
            StartingTheta = defaultMotion.Degree;
            StartingRho = defaultMotion.Radius;
            StartingPosition = new PolarPoint(StartingRho, StartingTheta).ToCoord();
            StartingHeight = defaultMotion.Height;
            StartingRotation = defaultMotion.Rotation;
        }
    }
}
