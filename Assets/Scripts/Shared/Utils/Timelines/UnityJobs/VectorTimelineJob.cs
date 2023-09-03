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
    public struct Vector3TimelineJob : IJob
    {
        [ReadOnly]
        public NativeArray<TimelineItemData<Vector3>> Items;
        public float CurrentTime;

        [WriteOnly]
        public NativeArray<Vector3> Result;

        public void Execute()
        {
            var delta = Vector3.zero;
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
        public static Vector3 Evaluate(float currentTime, TimelineItemData<Vector3>[] items)
        {
            using var itemArray = new NativeArray<TimelineItemData<Vector3>>(items.Length, Allocator.TempJob);
            using var resultArray = new NativeArray<Vector3>(1, Allocator.TempJob);
            var job = new Vector3TimelineJob()
            {
                CurrentTime = currentTime,
                Items = itemArray,
                Result = resultArray
            };
            job.Schedule().Complete();

            return resultArray[0];
        }
    }

    [BurstCompile]
    public struct Vector2TimelineJob : IJob
    {
        [ReadOnly]
        public NativeArray<TimelineItemData<Vector2>> Items;
        public float CurrentTime;

        [WriteOnly]
        public NativeArray<Vector2> Result;

        public void Execute()
        {
            var delta = Vector2.zero;
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
        public static Vector2 Evaluate(float currentTime, TimelineItemData<Vector2>[] items)
        {
            using var itemArray = new NativeArray<TimelineItemData<Vector2>>(items.Length, Allocator.TempJob);
            using var resultArray = new NativeArray<Vector2>(1, Allocator.TempJob);
            var job = new Vector2TimelineJob()
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
