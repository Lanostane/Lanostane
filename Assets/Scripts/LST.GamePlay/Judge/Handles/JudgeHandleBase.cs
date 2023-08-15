namespace LST.GamePlay.Judge
{
    public enum JudgeRoutine : byte
    {
        Continue,
        ForceStop,
        AddToFirstPass
    }

    public abstract class JudgeHandleBase
    {
        public abstract int TotalNoteCount { get; }
        public abstract float CurrentTiming { get; }
        public abstract float CurrentDegree { get; }
        public abstract bool JudgeDone { get; protected set; }
        public abstract bool IsInputAllowed(float chartTime);
        public abstract bool IsJudgeReportAllowed(float chartTime);
        public abstract JudgeRoutine ProcessInput(float chartTime, InputHandle handle);

        protected abstract JudgeType GetJudgeResult(float noteTime, float clickTime);

    }
}
