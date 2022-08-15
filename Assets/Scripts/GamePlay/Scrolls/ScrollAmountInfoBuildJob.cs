using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Scrolls
{
    [BurstCompile]
    internal struct ScrollAmountInfoBuildJob : IJobParallelFor
    {
        public Millisecond FromScroll;
        public Millisecond ToScroll;
        [ReadOnly] public NativeArray<Millisecond> Amounts;
        [WriteOnly] public NativeArray<ScrollAmountInfo> Results;

        public void Execute(int index)
        {
            var scrollAmount = Amounts[index];
            var inScreen = true;

            var uneasedP = Millisecond.InverseLerp(ToScroll, FromScroll, scrollAmount);
            var outscrollType = OutScrollType.Visible;
            if (scrollAmount > ToScroll)
            {
                inScreen = false;
                outscrollType = OutScrollType.Early;
            }
            else if (scrollAmount < FromScroll)
            {
                inScreen = false;
                outscrollType = OutScrollType.Late;
            }

            Results[index] = new ScrollAmountInfo()
            {
                Amount = scrollAmount,
                VisibleStatus = outscrollType,
                Progress = uneasedP,
                EasedProgress = inScreen ? Ease.GameSpaceEase(uneasedP) : uneasedP
            };
        }

        public static ScrollAmountInfo[] Run(Millisecond fromScroll, Millisecond toScroll, Millisecond[] amounts)
        {
            var length = amounts.Length;

            using var tAmountsNative = new NativeArray<Millisecond>(amounts, Allocator.TempJob);
            using var resultNative = new NativeArray<ScrollAmountInfo>(new ScrollAmountInfo[length], Allocator.TempJob);

            var buildJob = new ScrollAmountInfoBuildJob()
            {
                FromScroll = fromScroll,
                ToScroll = toScroll,
                Amounts = tAmountsNative,
                Results = resultNative
            };

            buildJob.Schedule(length, 2).Complete();
            return resultNative.ToArray();
        }

        public static void Run_NoAlloc(Millisecond fromScroll, Millisecond toScroll, Millisecond[] amounts, ScrollAmountInfo[] result)
        {
            var length = amounts.Length;

            using var tAmountsNative = new NativeArray<Millisecond>(amounts, Allocator.TempJob);
            using var resultNative = new NativeArray<ScrollAmountInfo>(result, Allocator.TempJob);

            var buildJob = new ScrollAmountInfoBuildJob()
            {
                FromScroll = fromScroll,
                ToScroll = toScroll,
                Amounts = tAmountsNative,
                Results = resultNative
            };

            buildJob.Schedule(length, 2).Complete();
            resultNative.CopyTo(result);
        }
    }
}
