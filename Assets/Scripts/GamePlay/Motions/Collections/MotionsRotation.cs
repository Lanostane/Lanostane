﻿using Charts;
using System.Linq;
using UnityEngine;

namespace GamePlay.Motions.Collections
{
    public struct RotationMotion : IMotion
    {
        public float Timing { get; set; }
        public float Duration;

        public float RotationDelta;

        public float StartRotation;
        public float EndRotation;
        public LST_Ease Ease;
    }

    public sealed class MotionsRotation : MotionCollection<RotationMotion>
    {
        public void AddMotion(LST_RotationMotion rot)
        {
            MotionDataHolder.Add(new()
            {
                Timing = rot.Timing,
                Duration = rot.Duration,
                Ease = rot.Ease,
                RotationDelta = rot.DeltaRotation
            });
        }

        public override void UpdateMotionAbsData()
        {
            TempHolder.Clear();

            var currentRotation = MotionManager.Instance.StartingRotation;
            foreach (var rot in MotionDataHolder.OrderBy(x => x.Timing))
            {
                var endRotation = currentRotation + rot.RotationDelta;
                var newRot = rot;
                newRot.StartRotation = currentRotation;
                newRot.EndRotation = endRotation;
                currentRotation = endRotation;

                TempHolder.Add(newRot);
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

        public override void UpdateMotion(RotationMotion currentMotion, float chartTime)
        {
            var rot = currentMotion;
            var p = GetProgress(rot.Timing, rot.Duration, chartTime);
            MotionManager.Instance.SetRotation(Mathf.Lerp(rot.StartRotation, rot.EndRotation, rot.Ease.EvalClamped(p)));
        }
    }
}
