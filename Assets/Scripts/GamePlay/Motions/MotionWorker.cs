using Lanostane.Charts;
using GamePlay.Motions.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Motions
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
        public readonly static Vector3 InitialXY = Vector3.zero;

        public GameCameraIndex CameraIndex;
        public Camera Cam;
        public Transform RotationOrigin;

        public float CurrentRotation { get; private set; }

        private readonly MotionsRotation _Rots = new();
        private readonly MotionsXY _XYs = new();
        private readonly MotionsHeight _Heights = new();

        public void Setup(LST_Chart chart)
        {
            if (CameraIndex == GameCameraIndex.Main)
            {
                foreach (var rot in chart.RotationMos)
                {
                    _Rots.AddMotion(rot);
                }
                _Rots.OnUpdateMotion += Update_Rotation;
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

            _XYs.OnUpdateMotion += Update_XY;
            _Heights.OnUpdateMotion += Update_Height;
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
            _XYs.UpdateMotionAbsData();
            _Heights.UpdateMotionAbsData();
            _Rots.UpdateMotionAbsData();
        }

        public void SetCameraTransform(Vector3 xyPos, float absHeight)
        {
            xyPos.z = absHeight;
            GameCamera.Transform.position = xyPos;
        }

        public void SetCameraPos(Vector3 xyPos)
        {
            var cameraPos = GameCamera.Transform.position;
            xyPos.z = cameraPos.z;
            Cam.transform.position = xyPos;
        }

        public void SetCameraPos(PolarPoint polar)
        {
            var height = GameCamera.Transform.position.z;
            var coord = polar.ToCoord();
            coord.z = height;
            Cam.transform.position = coord;
        }

        public void SetCameraHeight(float absHeight)
        {
            var cameraPos = GameCamera.Transform.position;
            cameraPos.z = absHeight;
            Cam.transform.position = cameraPos;
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

        public void UpdateChart(float chartTime)
        {
            _Rots.UpdateChartTime(chartTime);
            _XYs.UpdateChartTime(chartTime);
            _Heights.UpdateChartTime(chartTime);
        }

        public void CleanUp()
        {
            _Rots.Clear();
            _XYs.Clear();
            _Heights.Clear();
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

                    SetRotation(Mathf.Lerp(InitialRotation, MotionUpdater.Instance.StartingRotation, p));
                    SetCameraPos(Vector3.Lerp(InitialXY, MotionUpdater.Instance.StartingPosition, p));
                    SetCameraHeight(Mathf.Lerp(InitialHeight, MotionUpdater.Instance.StartingHeight, p));

                    time += Time.deltaTime;
                    yield return null;
                }

                SetRotation(MotionUpdater.Instance.StartingRotation);
                SetCameraPos(MotionUpdater.Instance.StartingPosition);
                SetCameraHeight(MotionUpdater.Instance.StartingHeight);
            }
        }
    }
}
