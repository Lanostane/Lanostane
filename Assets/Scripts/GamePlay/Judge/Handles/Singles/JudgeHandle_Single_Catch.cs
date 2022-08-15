using GamePlay.Judge.Inputs;
using Utils.Maths;

namespace GamePlay.Judge.Handles
{
    public class JudgeHandle_Single_Catch : SingleNoteJudgeHandle
    {
        #region Constants
        public const float Timeout = NoteJudgeManager.Timeout;
        public const float Size0Deg = NoteJudgeManager.Size0Deg;
        public const float Size1Deg = NoteJudgeManager.Size1Deg;
        public const float Size2Deg = NoteJudgeManager.Size1Deg;
        public const float JudgeAngleTolerance = NoteJudgeManager.JudgeAngleTolerance;
        #endregion

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
            return Timing <= chartTime;
        }

        public override JudgeRoutine ProcessInput(float chartTime, InputHandle handle)
        {
            if (!IsDegreeInRange(handle.GameAngle))
                return JudgeRoutine.Continue;

            switch (handle.EventType)
            {
                case InputEvent.PointerDown:
                    if (chartTime < Timing)
                    {
                        return JudgeRoutine.AddToFirstPass;
                    }
                    break;

                case InputEvent.PointerHold:
                    if (IsDegreeInRange(handle.GameAngle))
                    {
                        return JudgeRoutine.AddToFirstPass;
                    }
                    break;
            }

            return JudgeRoutine.Continue;
        }

        protected override JudgeType GetJudgeResult(float noteTime, float clickTime)
        {
            if (NoteJudgeManager.Instance.AutoPlay)
                return JudgeType.PerfectPlus;

            var delta = MathfE.AbsDelta(noteTime, clickTime);
            if (delta <= Timeout)
                return JudgeType.PerfectPlus;
            else
                return JudgeType.Miss;
        }
    }
}
