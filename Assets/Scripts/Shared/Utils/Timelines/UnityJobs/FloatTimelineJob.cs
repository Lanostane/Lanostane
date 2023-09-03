using PlasticPipe.PlasticProtocol.Messages;
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
    [BurstCompile]
    public struct FloatTimelineJob : IJob
    {
        [ReadOnly]
        public NativeArray<TimelineItemData<float>> Items;
        public float CurrentTime;

        [WriteOnly]
        public NativeArray<float> Result; 

        public void Execute()
        {
            var delta = 0.0f;
            var length = Items.Length;
            for (int i = 0; i < length; i++)
            {
                var item = Items[i];
                var startTime = item.Timing;
                var endTime = startTime + item.Duration;

                //Fully Done
                if (endTime <= CurrentTime)
                {
                    delta += item.Param;
                    continue;
                }

                //No Effect
                if (startTime >= CurrentTime)
                {
                    continue;
                }

                var p = math.unlerp(startTime, endTime, CurrentTime);
                p = item.Easing.EvalClamped(p);
                delta += item.Param * p;
            }

            Result[0] = delta;
        }

        [BurstDiscard]
        public static float Evaluate(float currentTime, TimelineItemData<float>[] items)
        {
            using var itemArray = new NativeArray<TimelineItemData<float>>(items.Length, Allocator.TempJob);
            using var resultArray = new NativeArray<float>(1, Allocator.TempJob);
            var job = new FloatTimelineJob()
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
