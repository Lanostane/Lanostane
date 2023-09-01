using Lanostane.Models;
using LST.GamePlay.Graphics;
using LST.GamePlay.Motions;
using System.Collections.Generic;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Judge
{
    public struct HoldJudgeSubNote
    {
        public bool IsFirst;
        public bool IsLast;
        public float Timing;
    }

    public class JudgeHandle_Long_Hold : LongNoteJudgeHandle
    {
        #region Constants
        public const float Timeout = JudgeConst.Timeout;
        #endregion

        public override int TotalNoteCount => _TotalSubNoteCount;
        public override float CurrentTiming => Mathf.Clamp(GamePlays.ChartPlayer.OffsetChartTime, Timing, Timing + Duration);
        public override float CurrentDegree => Graphic.HeadDegree;
        public bool IsDerailed => MathfE.AbsDelta(CurrentTiming, LastInputTime) > 0.4f;

        private readonly Queue<HoldJudgeSubNote> _JudgeTimings = new();
        private HoldJudgeSubNote? _CurrentJudgeTiming = null;
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

            if (!GamePlays.MotionUpdater.TryGetBPMByTime(info.Timing, out _BaseBPM))
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
            if (GamePlays.NoteJudgeUpdater.AutoPlay)
                return true;

            var min = Timing - Timeout;
            var max = Timing + Duration;
            if (min <= chartTime && chartTime <= max)
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

            if (!IsDerailed && chartTime >= Timing && !JudgeDone) //Not Derailed
            {
                Graphic.EnableJudgeEffect(JudgeType.PurePerfect);
            }
            else
            {
                Graphic.DisableJudgeEffect(JudgeType.PurePerfect);
            }
        }

        public override void UpdateJudge(float chartTime)
        {
            if (JudgeDone)
            {
                ResetInputTime();
                return;
            }

            if (MathfE.AbsDelta(CurrentTiming, LastInputTime) > 0.04f) //Derailed
            {
                ResetInputTime();
            }

            var subnote = _CurrentJudgeTiming.Value;
            var tolerance = (subnote.IsFirst || subnote.IsLast) ? _TickInterval * 5.0f : _TickInterval * 2.0f;

            if (GamePlays.NoteJudgeUpdater.AutoPlay || IsAutoJudgeNote)
            {
                if (subnote.Timing <= chartTime)
                {
                    ReportJudge(JudgeType.PurePerfect, subnote.IsLast);
                    TryDequeueTiming();
                }
                return;
            }

            if (subnote.Timing <= chartTime)
            {
                if (MathfE.AbsApprox(LastInputTime, subnote.Timing, tolerance))
                {
                    ReportJudge(JudgeType.PurePerfect, subnote.IsLast);
                    TryDequeueTiming();
                }
                else if (Timing + _TickInterval <= chartTime)
                {
                    ReportJudge(JudgeType.Miss, subnote.IsLast);
                    TryDequeueTiming();
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
