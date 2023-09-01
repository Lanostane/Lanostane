using Utils.Maths;

namespace LST.GamePlay.Judge
{
    public class JudgeHandle_Single_Catch : SingleNoteJudgeHandle
    {
        #region Constants
        public const float Timeout = JudgeConst.Timeout;
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
            if (GamePlays.NoteJudgeUpdater.AutoPlay || IsAutoJudgeNote)
                return JudgeType.PurePerfect;

            var delta = MathfE.AbsDelta(noteTime, clickTime);
            if (delta <= Timeout)
                return JudgeType.PurePerfect;
            else
                return JudgeType.Miss;
        }
    }
}
