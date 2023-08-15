using Lanostane.Models;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Maths;

namespace LST.Player.Motions
{
    public enum GameCameraIndex
    {
        Main,
        Sub_1,
        Sub_2,
        Sub_3
    }

    public interface IMotionWorker : IChartUpdater
    {
        void Setup(LST_Chart chart);
        void SortData();
        float CurrentRotation { get; }
        void StartDefaultMotion(float duration);
        void SetCameraTransform(Vector3 xyPos, float absHeight);
        void SetCameraPos(Vector3 xyPos);
        void SetCameraPos(PolarPoint polar);
        void SetCameraHeight(float absHeight);
        void SetRotation(float absRotation);
    }

    public class MotionWorker : MonoBehaviour, IMotionWorker
    {
        public const float InitialTheta = 0.0f;
        public const float InitialRho = 0.0f;
        public const float InitialHeight = -20.0f;
        public const float InitialRotation = 0.0f;
        public static readonly Vector3 InitialXY = Vector3.zero;

        public GameCameraIndex CameraIndex;
        public Transform CamTransform;

        [EnableIf(nameof(IsMainWorker))]
        public Transform RotationOrigin;

        public bool IsMainWorker => CameraIndex == GameCameraIndex.Main;
        public float CurrentRotation { get; private set; }

        private readonly MotionsRotation _RotMos = new();
        private readonly MotionsXY _XYMos = new();
        private readonly MotionsHeight _HeightMos = new();

        public void Setup(LST_Chart chart)
        {
            if (CameraIndex == GameCameraIndex.Main)
            {
                foreach (var rot in chart.RotationMos)
                {
                    _RotMos.AddMotion(rot);
                }
                _RotMos.OnUpdateMotion += Update_Rotation;
            }

            foreach (var xyl in chart.LinearMos)
            {
                _XYMos.AddMotion(xyl);
            }

            foreach (var xyc in chart.CirclerMos)
            {
                _XYMos.AddMotion(xyc);
            }

            foreach (var height in chart.HeightMos)
            {
                _HeightMos.AddMotion(height);
            }

            _XYMos.OnUpdateMotion += Update_XY;
            _HeightMos.OnUpdateMotion += Update_Height;
        }

        private void Update_Height(HeightMotion m, float p)
        {
            p = m.Ease.EvalClamped(p);
            SetCameraHeight(Mathf.Lerp(m.StartHeight, m.EndHeight, p));
        }

        private void Update_XY(XYMotion m, float p)
        {
            p = m.Ease.EvalClamped(p);
            if (m.IsLinear)
            {
                var startPos = m.Start.ToCoord();
                var endPos = m.End.ToCoord();
                SetCameraPos(Vector3.Lerp(startPos, endPos, p));
            }
            else
            {
                var newPos = PolarPoint.Lerp(m.Start, m.End, p);
                SetCameraPos(newPos);
            }
        }

        private void Update_Rotation(RotationMotion m, float p)
        {
            SetRotation(Mathf.Lerp(m.StartRotation, m.EndRotation, m.Ease.EvalClamped(p)));
        }

        public void SortData()
        {
            _XYMos.UpdateMotionAbsData();
            _HeightMos.UpdateMotionAbsData();
            _RotMos.UpdateMotionAbsData();
        }

        public void SetCameraTransform(Vector3 xyPos, float absHeight)
        {
            xyPos.z = absHeight;
            CamTransform.position = xyPos;
        }

        public void SetCameraPos(Vector3 xyPos)
        {
            var cameraPos = CamTransform.position;
            xyPos.z = cameraPos.z;
            CamTransform.position = xyPos;
        }

        public void SetCameraPos(PolarPoint polar)
        {
            var height = CamTransform.position.z;
            var coord = polar.ToCoord();
            coord.z = height;
            CamTransform.position = coord;
        }

        public void SetCameraHeight(float absHeight)
        {
            var cameraPos = CamTransform.position;
            cameraPos.z = absHeight;
            CamTransform.position = cameraPos;
        }

        public void SetRotation(float absRotation)
        {
            if (CameraIndex == GameCameraIndex.Main)
            {
                CurrentRotation = absRotation;
                RotationOrigin.localEulerAngles = new(0.0f, 0.0f, -absRotation);
            }
            else
            {
                Debug.LogError("SetRotation was called on non-MainCamera!");
            }
        }

        public void TimeUpdate(float chartTime)
        {
            _RotMos.UpdateChartTime(chartTime);
            _XYMos.UpdateChartTime(chartTime);
            _HeightMos.UpdateChartTime(chartTime);
        }

        public void CleanUp()
        {
            _RotMos.Clear();
            _XYMos.Clear();
            _HeightMos.Clear();
        }

        public void StartDefaultMotion(float duration)
        {
            StartCoroutine(DoDefaultMotion());

            IEnumerator DoDefaultMotion()
            {
                var time = 0.0f;
                while (time <= duration)
                {
                    var p = time / duration;
                    p = Ease.Sinusoidal.In(p);

                    SetRotation(Mathf.Lerp(InitialRotation, GamePlay.MotionUpdater.StartingRotation, p));
                    SetCameraPos(Vector3.Lerp(InitialXY, GamePlay.MotionUpdater.StartingPosition, p));
                    SetCameraHeight(Mathf.Lerp(InitialHeight, GamePlay.MotionUpdater.StartingHeight, p));

                    time += Time.deltaTime;
                    yield return null;
                }

                SetRotation(GamePlay.MotionUpdater.StartingRotation);
                SetCameraPos(GamePlay.MotionUpdater.StartingPosition);
                SetCameraHeight(GamePlay.MotionUpdater.StartingHeight);
            }
        }
    }
}
