using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Scrolls.Jobs
{
    public enum OutScrollType : byte
    {
        Visible,
        Early,
        Late
    }

    public struct ScrollAmountInfo
    {
        public Millisecond Amount;
        public float Progress;
        public float EasedProgress;
        public OutScrollType VisibleStatus;

        public bool IsVisible => VisibleStatus == OutScrollType.Visible;
    }

    [BurstCompile]
    public struct ScrollAmountQueryJob : IJobParallelFor
    {
        public int ScrollsLength;
        [ReadOnly] public NativeArray<float> Timings;
        [ReadOnly] public NativeArray<ScrollData> Scrolls;

        public NativeArray<Millisecond> TimeAmounts;

        public void Execute(int index)
        {
            for (int i = 0; i < ScrollsLength; i++)
            {
                var scroll = Scrolls[i];
                if (Timings[index] >= scroll.Timing)
                    TimeAmounts[index] += new Millisecond(scroll.GetPassedTime(Timings[index]) * scroll.Speed);
            }
        }

        public static ScrollAmountInfo[] Run(ScrollData[] scrolls, Millisecond fromScroll, Millisecond toScroll, float[] times)
        {
            if (scrolls == null || scrolls.Length <= 0)
                throw new ArgumentNullException(nameof(scrolls));

            if (times == null || times.Length <= 0)
                throw new ArgumentNullException(nameof(times));

            var length = times.Length;
            using var timingsNative = new NativeArray<float>(times, Allocator.TempJob);
            using var scrollsNative = new NativeArray<ScrollData>(scrolls, Allocator.TempJob);
            using var ctAmountNative = new NativeArray<float>(new float[] { 0.0f }, Allocator.TempJob);
            using var tAmountsNative = new NativeArray<Millisecond>(new Millisecond[length], Allocator.TempJob);

            using var resultNative = new NativeArray<ScrollAmountInfo>(new ScrollAmountInfo[length], Allocator.TempJob);

            var queryJob = new ScrollAmountQueryJob()
            {
                ScrollsLength = scrolls.Length,
                Timings = timingsNative,
                Scrolls = scrollsNative,
                TimeAmounts = tAmountsNative
            };

            var buildJob = new ScrollAmountInfoBuildJob()
            {
                FromScroll = fromScroll,
                ToScroll = toScroll,
                Amounts = tAmountsNative,
                Results = resultNative
            };

            buildJob.Schedule(length, 20, queryJob.Schedule(length, 15)).Complete();

            return resultNative.ToArray();
        }
    }
}
