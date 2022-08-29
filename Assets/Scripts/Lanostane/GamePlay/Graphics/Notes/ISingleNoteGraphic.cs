using Lst.Charts;
using Lst.GamePlay.Judge;
using Utils.Maths;

namespace Lst.GamePlay.Graphics
{
    public interface ISingleNoteGraphic
    {
        LST_SingleNoteType Type { get; }
        float Timing { get; }
        Millisecond ScrollTiming { get; }
        bool JudgeDone { get; set; }
        void Setup(LST_SingleNoteInfo info);
        void Show();
        void Hide();
        void TriggerJudgeEffect(JudgeType type);
        void SetNoteType(LST_SingleNoteType type);
        void UpdateProgress(float progress01);
        void DestroyInstance();
    }
}
