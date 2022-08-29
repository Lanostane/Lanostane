using Lst.Charts;
using GamePlay.Graphics.FX;
using GamePlay.Graphics.FX.Hold;
using GamePlay.Graphics.FX.Particles;
using GamePlay.Judge;
using GamePlay.Scrolls;
using System;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Graphics
{
    internal sealed class HoldNoteGraphic : MonoBehaviour, ILongNoteGraphic
    {
        public LST_LongNoteType Type { get; set; }
        public float Timing { get; set; }
        public Millisecond HeadScrollTiming { get; set; }
        public float Duration { get; set; }
        public bool JudgeStarted { get; set; }
        public bool JudgeDone { get; set; }
        public bool JudgeEffectEnabled { get; set; } = false;

        public float HeadDegree => JointInfo.GetDegreeByTime(ChartPlayer.OffsetChartTime);

        

        public Transform HeadRotationTransform;
        public Transform HeadPositionTransform;
        public GameObject HighlightObject;
        public SpriteRenderer NoteGraphic;
        public HoldLineRenderer LineRenderer;
        public LongNoteJointCollection JointInfo = new();

        private ILoopParticleFX _HeadParticle;
        private bool _IsHidden = false;

        public void Setup(LST_LongNoteInfo info)
        {
            Timing = info.Timing;
            HeadScrollTiming = ScrollManager.GetScrollTiming(Timing);
            Duration = info.Duration;
            SetNoteType(info.Type);
            HighlightObject.SetActive(info.Highlight);

            JointInfo.Setup(info);
            UpdateHeadRotation(info.Degree);
            UpdateHead(0.0f);

            LineRenderer.Setup(JointInfo);

            gameObject.SetActive(false);
            _IsHidden = true;
        }

        public void Show()
        {
            if (_IsHidden)
            {
                gameObject.SetActive(true);
                _IsHidden = false;
            }
        }

        public void Hide()
        {
            if (!_IsHidden)
            {
                gameObject.SetActive(false);
                _IsHidden = true;
            }
        }

        public void EnableJudgeEffect(JudgeType type)
        {
            if (_HeadParticle == null)
            {
                if (GameFX.HoldParticles.TryAllocate(out _HeadParticle))
                {
                    _HeadParticle.SetAutoRecycle(true);
                    _HeadParticle.SetTracking(HeadPositionTransform);
                    _HeadParticle.SetPosition(HeadPositionTransform.position);
                    _HeadParticle.StartEmit();
                }
            }

            JudgeEffectEnabled = true;
            LineRenderer.SetPressed(true);
        }

        public void DisableJudgeEffect(JudgeType type)
        {
            if (_HeadParticle != null)
            {
                _HeadParticle.StopEmit();
                _HeadParticle = null;
            }

            JudgeEffectEnabled = false;
            LineRenderer.SetPressed(false);
        }

        public void TriggerJudgeFinishedEffect(JudgeType type)
        {

        }

        public void SetNoteType(LST_LongNoteType type)
        {
            Type = type;
            NoteGraphic.color = type switch
            {
                LST_LongNoteType.Hold => new Color(0.2704254f, 0.5609212f, 0.9716981f), //Blue
                _ => throw new NotImplementedException(),
            };
        }

        public bool UpdateVisibility(float chartTime)
        {
            var visible = LineRenderer.IsInsideScreen();
            if (visible)
            {
                Show();
                return true;
            }
            else
            {
                Hide();
                return false;
            }
        }

        public void UpdateProgress(float headProgress01, float chartTime)
        {
            var headDeg = JointInfo.GetDegreeByProgress(GetHoldingProgression(chartTime));
            UpdateHeadRotation(headDeg);
            UpdateHead(headProgress01);

            LineRenderer.DoUpdate();
        }

        private float GetHoldingProgression(float chartTime)
        {
            return Mathf.InverseLerp(Timing, Timing + Duration, chartTime);
        }

        private void UpdateHeadRotation(float headDeg)
        {
            HeadRotationTransform.localEulerAngles = new Vector3(0.0f, 0.0f, headDeg);
        }

        private void UpdateHead(float progress01)
        {
            HeadPositionTransform.localScale = GameConst.LerpNoteSize(progress01) * Vector3.one;
            var pos = -GameConst.LerpSpace(progress01);
            HeadPositionTransform.localPosition = new Vector3(0.0f, pos, 0.0f);
        }

        public void DestroyInstance()
        {
            Destroy(this);
        }
    }
}
