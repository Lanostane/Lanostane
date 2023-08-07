using Utils.Maths;

namespace LST.Player.Judge
{
    public class JudgeHandle_Single_Click : SingleNoteJudgeHandle
    {
        #region Constants
        public const float Timeout = JudgeConst.Timeout;
        public const float TapPerfectPlus = JudgeConst.TapPerfectPlus;
        public const float TapPerfect = JudgeConst.TapPerfect;
        public const float TapGood = JudgeConst.TapGood;
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
            return true;
        }

        public override JudgeRoutine ProcessInput(float inputTime, InputHandle handle)
        {
            if (!IsDegreeInRange(handle.GameAngle))
                return JudgeRoutine.Continue;

            if (handle.EventType == InputEvent.PointerDown && IsDegreeInRange(handle.GameAngle))
            {
                return JudgeRoutine.AddToFirstPass;
            }

            return JudgeRoutine.Continue;
        }

        protected override JudgeType GetJudgeResult(float noteTime, float clickTime)
        {
            if (GamePlayManager.NoteJudgeUpdater.AutoPlay)
                return JudgeType.PurePerfect;

            var delta = MathfE.AbsDelta(noteTime, clickTime);
            if (delta <= TapPerfectPlus)
                return JudgeType.PurePerfect;
            else if (delta <= TapPerfect)
                return JudgeType.Perfect;
            else if (delta <= TapGood)
                return JudgeType.Good;
            else
                return JudgeType.Miss;
        }
    }
}
