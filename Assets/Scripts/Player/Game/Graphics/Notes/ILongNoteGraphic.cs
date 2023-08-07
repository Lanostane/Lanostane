using Lanostane.Charts;
using Utils.Maths;
using LST.Player.Judge;

namespace LST.Player.Graphics
{
    public interface ILongNoteGraphic
    {
        LST_LongNoteType Type { get; }
        float Timing { get; }
        Millisecond HeadScrollTiming { get; }
        float Duration { get; }
        float HeadDegree { get; }
        bool JudgeStarted { get; set; }
        bool JudgeDone { get; set; }
        void Setup(LST_LongNoteInfo info);
        void Show();
        void Hide();
        bool JudgeEffectEnabled { get; }
        void EnableJudgeEffect(JudgeType type);
        void DisableJudgeEffect(JudgeType type);
        void TriggerJudgeFinishedEffect(JudgeType type);
        void SetNoteType(LST_LongNoteType type);
        bool UpdateVisibility(float chartTime);
        void UpdateProgress(float headProgress01, float chartTime);
        void DestroyInstance();
    }
}
