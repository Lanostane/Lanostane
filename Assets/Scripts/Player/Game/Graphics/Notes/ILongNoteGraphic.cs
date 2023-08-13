using Lanostane.Models;
using Utils.Maths;
using LST.Player.Judge;
using LST.Player.Scrolls;

namespace LST.Player.Graphics
{
    public interface ILongNoteGraphic : INoteGraphic
    {
        LST_LongNoteType Type { get; }
        float Timing { get; }
        ScrollTiming HeadScrollTiming { get; }
        float Duration { get; }
        float HeadDegree { get; }
        bool JudgeStarted { get; set; }
        bool JudgeDone { get; set; }
        void Setup(LST_LongNoteInfo info);
        bool JudgeEffectEnabled { get; }
        void EnableJudgeEffect(JudgeType type);
        void DisableJudgeEffect(JudgeType type);
        void TriggerJudgeFinishedEffect(JudgeType type);
        void SetNoteType(LST_LongNoteType type);
        bool UpdateVisibility(float chartTime);
        void UpdateProgress(float headProgress01, float chartTime);
    }
}
