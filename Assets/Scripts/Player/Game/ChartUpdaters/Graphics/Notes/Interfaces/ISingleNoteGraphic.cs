using Lanostane.Models;
using Utils.Maths;
using LST.Player.Judge;
using LST.Player.Scrolls;

namespace LST.Player.Graphics
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
