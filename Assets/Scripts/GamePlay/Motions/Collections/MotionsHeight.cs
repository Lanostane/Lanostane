using Charts;
using System.Linq;
using UnityEngine;

namespace GamePlay.Motions.Collections
{
    public struct HeightMotion : IMotion
    {
        public float Timing { get; set; }
        public float Duration;

        public float HeightDelta;

        public float StartHeight;
        public float EndHeight;
        public LST_Ease Ease;
    }

    public sealed class MotionsHeight : MotionCollection<HeightMotion>
    {
        public void AddMotion(LST_HeightMotion heightMo)
        {
            MotionDataHolder.Add(new()
            {
                Timing = heightMo.Timing,
                Duration = heightMo.Duration,
                Ease = heightMo.Ease,
                HeightDelta = heightMo.DeltaHeight
            });
        }

        public override void UpdateMotionAbsData()
        {
            TempHolder.Clear();

            var currentHeight = MotionManager.Instance.StartingHeight;
            foreach (var height in MotionDataHolder.OrderBy(x => x.Timing))
            {
                var newHeight = height;
                var endHeight = currentHeight + height.HeightDelta;
                newHeight.StartHeight = currentHeight;
                newHeight.EndHeight = endHeight;
                currentHeight = endHeight;

                TempHolder.Add(newHeight);
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

        public override void UpdateMotion(HeightMotion currentMotion, float chartTime)
        {
            var height = currentMotion;
            var p = height.Ease.EvalClamped(GetProgress(height.Timing, height.Duration, chartTime));
            MotionManager.Instance.SetCameraHeight(Mathf.Lerp(height.StartHeight, height.EndHeight, p));
        }
    }
}
