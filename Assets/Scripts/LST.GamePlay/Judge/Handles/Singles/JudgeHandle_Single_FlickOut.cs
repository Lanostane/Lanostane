﻿using Lanostane.Models;
using Utils;
using Utils.Maths;

namespace LST.GamePlay.Judge
{
    public class JudgeHandle_Single_FlickOut : SingleNoteJudgeHandle
    {
        #region Constants
        public const float Timeout = JudgeConst.Timeout;
        public const float FlickPerfect = JudgeConst.FlickPerfect;
        #endregion

        public bool FlickDone = false;

        public override bool IsInputAllowed(float chartTime)
        {
            if (MathfE.AbsApprox(Timing, chartTime, Timeout))
            {
                return true;
            }
            return false;
        }

        public override bool IsJudgeReportAllowed(float chartTime)
        {
            return FlickDone || GamePlays.NoteJudgeUpdater.AutoPlay;
        }

        public override JudgeRoutine ProcessInput(float chartTime, InputHandle handle)
        {
            if (!IsDegreeInRange(handle.GameAngle))
                return JudgeRoutine.Continue;

            if (handle.EventType == InputEvent.PointerDown)
            {
                return JudgeRoutine.AddToFirstPass;
            }

            if (handle.EventType == InputEvent.Flick)
            {
                if (handle.HasHandledFlick && handle.LastFlickDir == LST_FlickDir.Out)
                    return JudgeRoutine.Continue;

                var delta = MathfE.AbsDeltaAngle(handle.GameFlickAngle, Degree - 180.0f);
                if (delta < 35.0f)
                {
                    FlickDone = true;
                    return JudgeRoutine.AddToFirstPass;
                }
            }

            return JudgeRoutine.Continue;
        }

        protected override void OnJudgeReported(JudgeType type, InputHandle handle = null)
        {
            if (handle != null)
            {
                handle.HasHandledFlick = true;
                handle.LastFlickDir = LST_FlickDir.Out;
            }
        }

        protected override JudgeType GetJudgeResult(float noteTime, float clickTime)
        {
            if (GamePlays.NoteJudgeUpdater.AutoPlay || IsAutoJudgeNote)
                return JudgeType.PurePerfect;

            var delta = MathfE.AbsDelta(noteTime, clickTime);
            if (delta <= FlickPerfect)
                return JudgeType.PurePerfect;
            else
                return JudgeType.Good;
        }
    }
}
