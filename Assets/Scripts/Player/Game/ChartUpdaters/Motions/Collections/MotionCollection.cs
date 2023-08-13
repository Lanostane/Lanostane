using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LST.Player.Motions
{
    public abstract class MotionCollection<M> where M : struct, IMotion
    {
        protected readonly List<M> MotionDataHolder = new();
        protected readonly List<M> TempHolder = new();

        public event Action<M, float> OnUpdateMotion;

        public abstract void UpdateMotionAbsData();
        public abstract void UpdateChartTime(float chartTime);
        protected void UpdateMotion(M currentMotion, float chartTime)
        {
            var p = GetProgress(currentMotion.Timing, currentMotion.Duration, chartTime);
            OnUpdateMotion?.Invoke(currentMotion, p);
        }

        public float GetProgress(float timing, float duration, float time)
        {
            var endTiming = timing + duration;

            if (time <= timing)
                return 0.0f;

            if (time >= endTiming)
                return 1.0f;

            if (Mathf.Approximately(duration, 0.0f))
            {
                if (time < timing) return 0.0f;
                else return 1.0f;
            }
            return Mathf.InverseLerp(timing, endTiming, time);
        }

        public bool TryGetLastMotion(float timing, out M motion)
        {
            var index = -1;
            var length = MotionDataHolder.Count;
            for (int i = 0; i < length; i++)
            {
                var item = MotionDataHolder[i];
                if (item.Timing < timing && i > index)
                {
                    index = i;
                }
            }

            if (index > -1)
            {
                motion = MotionDataHolder[index];
                return true;
            }
            else
            {
                motion = default;
                return false;
            }
        }

        public void Clear()
        {
            MotionDataHolder.Clear();
            TempHolder.Clear();
        }
    }
}
