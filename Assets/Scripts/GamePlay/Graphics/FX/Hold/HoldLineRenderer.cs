using GamePlay.Scrolls;
using System;
using System.Linq;
using UnityEngine;
using Utils;

namespace GamePlay.Graphics.FX.Hold
{
    public struct JointInfo
    {
        public MiliSec ScrollTiming;
        public Vector3 Direction;
        public GameObject JointObject;
        public Transform JointTransform;
    }

    public struct LinePointInfo
    {
        public float Timing;
        public Vector3 LeftDir;
        public Vector3 RightDir;

        public static Vector3 DegreeToDir(float degree)
        {
            var x = FastTrig.Sin(degree);
            var y = -FastTrig.Cos(degree);

            return new Vector3(x, y, 0.0f);
        }
    }

    public class HoldLineRenderer : MonoBehaviour
    {
        public const bool HAS_JOINT = true;

        public MeshRenderer Renderer;
        public MeshFilter Filter;

        public Material DefaultMaterial;
        public Material HoldMaterial;
        public GameObject JointPrefab;

        public float Width = 4.15f;

        private LongNoteJointCollection _JointInfo;
        private JointInfo[] _JointNoteInfos = Array.Empty<JointInfo>();
        private LinePointInfo[] _PointInfos;
        private MiliSec[] _ScrollAmounts;
        private Vector3[] _VerticsBuffer;
        private Mesh _Mesh = null;
        private bool _IsPressed = false;

        private MiliSec _MinAmount;
        private MiliSec _MaxAmount;

        public void Setup(LongNoteJointCollection jointInfo)
        {
            _JointInfo = jointInfo;

            var pointList = TempList<LinePointInfo>.GetList();
            var jointNoteList = TempList<JointInfo>.GetList();
            var firstNote = true;
            foreach (var joint in _JointInfo)
            {
                if (!firstNote)
                {
                    var jointNote = Instantiate(JointPrefab, transform);
                    jointNote.transform.localEulerAngles = new Vector3(0.0f, 0.0f, joint.StartDeg);
                    jointNoteList.Add(new()
                    {
                        ScrollTiming = ScrollManager.GetScrollTiming(joint.StartTiming),
                        JointObject = jointNote,
                        JointTransform = jointNote.transform,
                        Direction = LinePointInfo.DegreeToDir(joint.StartDeg)
                    });
                }
                firstNote = false;

                var smoothness = Mathf.Abs(joint.DeltaDegree) * 2.0f;
                var smoothness2 = joint.Duration * 100.0f;

                var count = (int)Mathf.Max(smoothness + smoothness2, 100.0f);
                var delta = joint.Duration / count;

                for (int i = 0; i < count; i++)
                {
                    var time = joint.StartTiming + (delta * i);
                    var deg = _JointInfo.GetDegreeByTime(time);
                    var x = FastTrig.Sin(deg);
                    var y = -FastTrig.Cos(deg);
                    var dir = new Vector3(x, y, 0.0f);

                    pointList.Add(new()
                    {
                        Timing = time,
                        LeftDir = LinePointInfo.DegreeToDir(deg + Width),
                        RightDir = LinePointInfo.DegreeToDir(deg - Width)
                    });
                }
            }

            _PointInfos = pointList.ToArray();
            _JointNoteInfos = jointNoteList.ToArray();
            var length = _PointInfos.Length;
            if (length < 2)
            {
                Debug.LogError($"Hold Note only have {length} Points???");
            }

            var timings = _PointInfos.Select(point => point.Timing).ToArray();
            var amountInfos = ScrollManager.GetProgressBulk(0.0f, timings);
            var sorted = amountInfos.OrderBy(x => x.Amount);
            _ScrollAmounts = amountInfos.Select(x => x.Amount).ToArray();
            _MinAmount = sorted.First().Amount;
            _MaxAmount = sorted.Last().Amount;

            _Mesh = new Mesh();
            Filter.mesh = _Mesh;
            CreateMeshJob.Create(_JointInfo.StartTiming, _PointInfos, _Mesh);

            _VerticsBuffer = new Vector3[_Mesh.vertices.Length];
        }

        void OnDestroy()
        {
            if (_Mesh != null)
            {
                Destroy(_Mesh);
            }
        }

        public void SetPressed(bool pressed)
        {
            if (_IsPressed != pressed)
            {
                if (pressed) Renderer.sharedMaterial = HoldMaterial;
                else Renderer.sharedMaterial = DefaultMaterial;

                _IsPressed = pressed;
            }
        }

        public void DoUpdate()
        {
            UpdateMeshJob.UpdateVertics(_ScrollAmounts, _PointInfos, _VerticsBuffer);
            _Mesh.SetVertices(_VerticsBuffer);
            _Mesh.RecalculateBounds();

            var length = _JointNoteInfos.Length;
            for (int i = 0; i < length; i++)
            {
                var jointNote = _JointNoteInfos[i];
                var p = ScrollManager.Instance.GetProgressionSingleFast(jointNote.ScrollTiming, out var visible);
                if (visible)
                {
                    p = Ease.GameSpaceEase(p);
                    jointNote.JointTransform.localPosition = jointNote.Direction * GameConst.LerpSpace(p);
                    jointNote.JointTransform.localScale = Vector3.one * GameConst.LerpNoteSize(p);
                    jointNote.JointObject.SetActive(true);
                }
                else
                {
                    jointNote.JointObject.SetActive(false);
                }
            }
        }

        public bool IsInsideScreen()
        {
            return ScrollManager.IsScrollRangeVisible(_MinAmount, _MaxAmount);
        }
    }
}
