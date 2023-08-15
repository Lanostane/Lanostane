using Lanostane.Models;
using Utils.Maths;
using LST.GamePlay.Judge;
using LST.GamePlay.Scrolls;

namespace LST.GamePlay.Graphics
{
    public interface ISingleNoteGraphic : INoteGraphic
    {
        LST_SingleNoteType Type { get; }
        float Timing { get; }
        ScrollTiming ScrollTiming { get; }
        bool JudgeDone { get; set; }
        void Setup(LST_SingleNoteInfo info);
        void TriggerJudgeEffect(JudgeType type);
        void SetNoteType(LST_SingleNoteType type);
        void UpdateProgress(float progress01);
    }
}
