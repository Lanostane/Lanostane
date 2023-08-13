using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Utils.Maths;

namespace LST.Player.Scrolls
{
    [BurstCompile]
    public struct ScrollProgressUpdateJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<ScrollTiming> Timings;
        [ReadOnly] public NativeHashMap<ushort, ScrollRangeData> ScrollRangeNativeData;
        public NativeArray<ScrollProgress> Result;

        public void Execute(int index)
        {
            var timing = Timings[index];
            var rangeData = ScrollRangeNativeData[timing.ScrollGroupID];
            Result[index] = ScrollProgress.Create(rangeData.From, rangeData.To, timing.Timing);
        }

        public static void Update(ScrollTiming[] param, ScrollProgress[] progressResult)
        {
            using var paramsNative = new NativeArray<ScrollTiming>(param, Allocator.TempJob);
            using var resultNative = new NativeArray<ScrollProgress>(param.Length, Allocator.TempJob);
            var newJob = new ScrollProgressUpdateJob()
            {
                Timings = paramsNative,
                Result = resultNative
            };

            GamePlayManager.ScrollUpdater.GetNativeScrollRangeData(ref newJob.ScrollRangeNativeData);
            newJob.Schedule(param.Length, 8).Complete();
            resultNative.CopyTo(progressResult);
        }
    }
}
