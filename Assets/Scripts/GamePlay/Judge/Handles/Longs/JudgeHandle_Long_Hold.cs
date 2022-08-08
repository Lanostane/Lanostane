using Charts;
using GamePlay.Graphics;
using GamePlay.Judge.Inputs;
using GamePlay.Motions;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace GamePlay.Judge.Handles
{
    public struct HoldJudgeTimingInfo
    {
        public bool IsFirst;
        public bool IsLast;
        public float Timing;
    }

    public class JudgeHandle_Long_Hold : LongNoteJudgeHandle
    {
        #region Constants
        public const float Timeout = NoteJudgeManager.Timeout;
        public const float TapPerfect = NoteJudgeManager.TapPerfect;
        public const float FlickPerfect = NoteJudgeManager.FlickPerfect;
        public const float TapGood = NoteJudgeManager.TapGood;
        public const float FlickGood = NoteJudgeManager.FlickGood;
        public const float Size0Deg = NoteJudgeManager.Size0Deg;
        public const float Size1Deg = NoteJudgeManager.Size1Deg;
        public const float Size2Deg = NoteJudgeManager.Size1Deg;
        public const float JudgeAngleTolerance = NoteJudgeManager.JudgeAngleTolerance;
        #endregion

        public override int TotalNoteCount => _TotalSubNoteCount;
        public override float CurrentTiming => Mathf.Clamp(ChartPlayer.OffsetChartTime, Timing, Timing + Duration);
        public override float CurrentDegree => Graphic.HeadDegree;

        private readonly Queue<HoldJudgeTimingInfo> _JudgeTimings = new();
        private HoldJudgeTimingInfo? _CurrentJudgeTiming = null;
        private int _TotalSubNoteCount;
        private float _BaseBPM = 0.0f;
        private float _TickInterval = 0.0f;

        public override void Setup(LST_LongNoteInfo info, ILongNoteGraphic graphic)
        {
            base.Setup(info, graphic);

            _JudgeTimings.Enqueue(new()
            {
                IsFirst = true,
                IsLast = false,
                Timing = info.Timing
            });

            if (!MotionManager.Instance.TryGetBPMByTime(info.Timing, out _BaseBPM))
            {
                _BaseBPM = 100.0f;
                Debug.LogError("BPM Info was none! Falling back to 100.0...");
            }

            _TickInterval = 30.0f / _BaseBPM;
            var absDuration = info.Duration;
            var i = 1;
            while (true)
            {
                var timeTemp = _TickInterval * i;
                if (timeTemp >= absDuration || MathfE.AbsApprox(timeTemp, absDuration, 0.0003f))
                {
                    break;
                }

                _JudgeTimings.Enqueue(new()
                {
                    IsFirst = false,
                    IsLast = false,
                    Timing = timeTemp + info.Timing
                });

                i++;
            }

            _JudgeTimings.Enqueue(new()
            {
                IsFirst = false,
                IsLast = true,
                Timing = info.Timing + info.Duration
            });

            _TotalSubNoteCount = _JudgeTimings.Count;
            TryDequeueTiming();
        }

        public override bool IsInputAllowed(float chartTime)
        {
            if (NoteJudgeManager.Instance.AutoPlay)
                return true;

            if (chartTime >= Timing - Timeout)
                return true;

            if (chartTime <= Timing + Duration)
                return true;

            return false;
        }

        public override bool IsJudgeReportAllowed(float chartTime)
        {
            return Timing <= chartTime;
        }

        public override JudgeRoutine ProcessInput(float chartTime, InputHandle handle)
        {
            if (!IsDegreeInRange(handle.GameAngle))
                return JudgeRoutine.Continue;

            if (LastInputID > -1 && handle.ID != LastInputID)
                return JudgeRoutine.Continue;

            var firstTimePassed = Timing - chartTime < -0.1f;
            var singleInputRoutine = firstTimePassed ? JudgeRoutine.Continue : JudgeRoutine.AddToFirstPass;
            return handle.EventType switch
            {
                //These actions are blocked by rail
                InputEvent.PointerDown => singleInputRoutine,
                InputEvent.Flick => singleInputRoutine,
                InputEvent.PointerHold => JudgeRoutine.AddToFirstPass,

                _ => JudgeRoutine.Continue,
            };
        }

        public override void UpdateGraphic(float chartTime)
        {
            if (JudgeDone && !Graphic.JudgeEffectEnabled)
                return;

            if (MathfE.AbsDelta(CurrentTiming, LastInputTime) <= 0.04 && !JudgeDone) //Not Derailed
            {
                Graphic.EnableJudgeEffect(JudgeType.Perfect);
            }
            else
            {
                Graphic.DisableJudgeEffect(JudgeType.Perfect);
            }
        }

        public override void UpdateJudge(float chartTime)
        {
            if (JudgeDone)
            {
                ResetInputTime();
                return;
            }

            if (MathfE.AbsDelta(CurrentTiming, LastInputTime) > 0.04) //Derailed
            {
                ResetInputID();
            }

            if (_CurrentJudgeTiming.HasValue)
            {
                var timingInfo = _CurrentJudgeTiming.Value;

                var tolerance = (timingInfo.IsFirst || timingInfo.IsLast) ? _TickInterval * 3.5f : _TickInterval * 2.5f;
                if (timingInfo.Timing > chartTime)
                {
                    return;
                }

                if (NoteJudgeManager.Instance.AutoPlay)
                {
                    if (timingInfo.Timing <= chartTime)
                    {
                        ReportJudge(JudgeType.Perfect, timingInfo.IsLast);
                        TryDequeueTiming();
                    }
                    return;
                }

                if (MathfE.AbsApprox(LastInputTime, timingInfo.Timing, tolerance))
                {
                    if (LastInputType == InputEvent.PointerHold)
                    {
                        ReportJudge(JudgeType.Perfect, timingInfo.IsLast);
                        TryDequeueTiming();
                    }
                }
                else if (chartTime > Timing + tolerance)
                {
                    if (timingInfo.Timing < chartTime)
                    {
                        ReportJudge(JudgeType.Miss, timingInfo.IsLast);
                        TryDequeueTiming();
                    }
                }
            }
        }

        private void TryDequeueTiming()
        {
            if (_JudgeTimings.Count > 0)
            {
                _CurrentJudgeTiming = _JudgeTimings.Dequeue();
            }
            else
            {
                _CurrentJudgeTiming = null;
            }
        }

        protected override JudgeType GetJudgeResult(float noteTime, float clickTime)
        {
            return JudgeType.Perfect;
        }
    }
}
