﻿using Charts;
using System.Linq;

namespace GamePlay.Motions.Collections
{
    public struct BpmMotion : IMotion
    {
        public float Timing { get; set; }
        public float Bpm;
    }

    public sealed class MotionsBpm : MotionCollection<BpmMotion>
    {
        public float CurrentBPM { get; private set; }

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

            var currentRotation = MotionManager.Instance.StartingRotation;
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

        public override void UpdateMotion(BpmMotion currentMotion, float chartTime)
        {
            CurrentBPM = currentMotion.Bpm;
        }
    }
}
