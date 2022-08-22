using GamePlay.Judge.Inputs;
using Utils.Maths;

namespace GamePlay.Judge.Handles
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
            if (NoteJudgeManager.Instance.AutoPlay)
                return JudgeType.PerfectPlus;

            var delta = MathfE.AbsDelta(noteTime, clickTime);
            if (delta <= TapPerfectPlus)
                return JudgeType.PerfectPlus;
            else if (delta <= TapPerfect)
                return JudgeType.Perfect;
            else if (delta <= TapGood)
                return JudgeType.Good;
            else
                return JudgeType.Miss;
        }
    }
}
