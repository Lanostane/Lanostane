using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Utils.Maths;

namespace Utils.Timelines
{
    public struct PolarPointTimelineItem
    {
        public float Timing;
        public float Duration;
        public PolarPoint Delta;
        public EaseType Easing;
    }

    [BurstCompile]
    public struct PolarPointTimelineJob : IJob
    {
        [ReadOnly]
        public NativeArray<PolarPointTimelineItem> Items;
        public float CurrentTime;

        [WriteOnly]
        public NativeArray<PolarPoint> Result;

        public void Execute()
        {
            var delta = new PolarPoint(0.0f, 0.0f);
            var length = Items.Length;
            for (int i = 0; i < length; i++)
            {
                var item = Items[i];
                var startTime = item.Timing;
                var endTime = startTime + item.Duration;

                //Fully Done
                if (endTime <= CurrentTime)
                {
                    delta += item.Delta;
                    continue;
                }

                //No Effect
                if (startTime >= CurrentTime)
                {
                    continue;
                }

                var p = math.unlerp(startTime, endTime, CurrentTime);
                p = item.Easing.EvalClamped(p);
                delta += item.Delta * p;
            }

            Result[0] = delta;
        }

        [BurstDiscard]
        public static PolarPoint Evaluate(float currentTime, PolarPointTimelineItem[] items)
        {
            using var itemArray = new NativeArray<PolarPointTimelineItem>(items.Length, Allocator.TempJob);
            using var resultArray = new NativeArray<PolarPoint>(1, Allocator.TempJob);
            var job = new PolarPointTimelineJob()
            {
                CurrentTime = currentTime,
                Items = itemArray,
                Result = resultArray
            };
            job.Schedule().Complete();

            return resultArray[0];
        }
    }
}
