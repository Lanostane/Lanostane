using Charts;
using GamePlay.Motions.Collections;
using UnityEngine;
using Utils;

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

        void AddMotions(LST_Chart chart);
        void UpdateAbsValue();
        bool TryGetBPMByTime(float time, out float bpm);
        void SetDefaultMotion(LST_DefaultMotion defaultMotion);
        void SetCameraTransform(Vector3 xyPos, float absHeight);
        void SetCameraPos(Vector3 xyPos);
        void SetCameraPos(PolarPoint polar);
        void SetCameraHeight(float absHeight);
        void SetRotation(float absRotation);
    }

    internal class MotionManager : MonoBehaviour, IMotionManager
    {
        public static IMotionManager Instance { get; private set; }

        public Transform RotationOrigin;
        public Transform CameraTransform;

        public float CurrentBPM { get; private set; }
        public float CurrentRotation { get; private set; }
        public float StartingTheta { get; private set; }
        public float StartingRho { get; private set; }
        public float StartingHeight { get; private set; }
        public Vector3 StartingPosition { get; private set; }
        public PolarPoint StartingPolar => new(StartingRho, StartingTheta);
        public float StartingRotation { get; private set; }

        private readonly MotionsRotation _Rots = new();
        private readonly MotionsXY _XYs = new();
        private readonly MotionsHeight _Heights = new();
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
            foreach (var rot in chart.RotationMos)
            {
                _Rots.AddMotion(rot);
            }

            foreach (var xyl in chart.LinearMos)
            {
                _XYs.AddMotion(xyl);
            }

            foreach (var xyc in chart.CirclerMos)
            {
                _XYs.AddMotion(xyc);
            }

            foreach (var height in chart.HeightMos)
            {
                _Heights.AddMotion(height);
            }

            foreach (var bpm in chart.BPMs)
            {
                _BPMS.AddBpmChange(bpm);
            }
        }

        public void UpdateAbsValue()
        {
            _Rots.UpdateMotionAbsData();
            _XYs.UpdateMotionAbsData();
            _Heights.UpdateMotionAbsData();
            _BPMS.UpdateMotionAbsData();
        }

        public void UpdateChart(float chartTime)
        {
            _Rots.UpdateChartTime(chartTime);
            _XYs.UpdateChartTime(chartTime);
            _Heights.UpdateChartTime(chartTime);
            _BPMS.UpdateChartTime(chartTime);

            if (_BPMS.CurrentBPM != CurrentBPM)
            {
                BPM.Invoke_BPMChange(_BPMS.CurrentBPM);
                CurrentBPM = _BPMS.CurrentBPM;
            }
        }

        public void CleanUp()
        {
            _Rots.Clear();
            _XYs.Clear();
            _Heights.Clear();
            _BPMS.Clear();

            StartingTheta = 0.0f;
            StartingHeight = -20.0f;
            StartingPosition = Vector3.zero;
            StartingRho = 0.0f;
            StartingRotation = 0.0f;
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
            SetCameraTransform(StartingPosition, StartingHeight);

            StartingRotation = defaultMotion.Rotation;
            SetRotation(StartingRotation);
        }

        public void SetCameraTransform(Vector3 xyPos, float absHeight)
        {
            xyPos.z = absHeight;
            CameraTransform.position = xyPos;
        }

        public void SetCameraPos(Vector3 xyPos)
        {
            var cameraPos = CameraTransform.position;
            xyPos.z = cameraPos.z;
            CameraTransform.position = xyPos;
        }

        public void SetCameraPos(PolarPoint polar)
        {
            var height = CameraTransform.position.z;
            var coord = polar.ToCoord();
            coord.z = height;
            CameraTransform.position = coord;
        }

        public void SetCameraHeight(float absHeight)
        {
            var cameraPos = CameraTransform.position;
            cameraPos.z = absHeight;
            CameraTransform.position = cameraPos;
        }

        public void SetRotation(float absRotation)
        {
            CurrentRotation = absRotation;
            RotationOrigin.localEulerAngles = new(0.0f, 0.0f, -absRotation);
        }
    }
}
