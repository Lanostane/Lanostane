using Lanostane.Models;
using System.Linq;

namespace LST.GamePlay.Motions
{
    public struct BpmMotion : IMotion
    {
        public float Timing { get; set; }
        public readonly float Duration => 0.0f;
        public float Bpm;
    }

    public sealed class MotionsBpm : MotionCollection<BpmMotion>
    {
        public void AddBpmChange(LST_BPMChange bpm)
        {
            MotionDataHolder.Add(new()
            {
                Timing = bpm.Timing,
                Bpm = bpm.BPM
            });
        }

        public override void UpdateMotionAbsData()
        {
            TempHolder.Clear();

            var currentRotation = GamePlays.MotionUpdater.StartingRotation;
            foreach (var bpm in MotionDataHolder.OrderBy(x => x.Timing))
            {
                TempHolder.Add(bpm);
            }

            MotionDataHolder.Clear();
            MotionDataHolder.AddRange(TempHolder);
        }

        public override void UpdateChartTime(float chartTime)
        {
            if (TryGetLastMotion(chartTime, out var motion))
            {
                UpdateMotion(motion, chartTime);
            }
        }
    }
}
