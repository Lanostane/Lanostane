using GamePlay.Scrolls;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Utils;

namespace GamePlay.Graphics.FX.Hold
{
    [BurstCompile]
    public struct UpdateMeshJob : IJobParallelFor
    {
        public const float SIZE = 4.15f;

        public MiliSec FromScroll;
        public MiliSec ToScroll;

        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<MiliSec> ScrollAmounts;
        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<LinePointInfo> Points;
        [NativeDisableParallelForRestriction]
        [WriteOnly] public NativeArray<Vector3> Vertics;

        public void Execute(int index)
        {
            var amount = ScrollAmounts[index];
            var point = Points[index];
            var progress = MiliSec.InverseLerp(ToScroll, FromScroll, amount);
            if (progress > 0.0f && progress < 1.0f)
            {
                progress = Ease.GameSpaceEase(progress);
            }

            var radius = GameConst.LerpSpace(progress);

            Vertics[index * 2] = point.LeftDir * radius;
            Vertics[(index * 2) + 1] = point.RightDir * radius;
        }

        public static void UpdateVertics(MiliSec[] amounts, LinePointInfo[] points, Vector3[] result)
        {
            var length = amounts.Length;
            var trigLength = length * 2;

            using var scrollAmountsNative = new NativeArray<MiliSec>(amounts, Allocator.TempJob);
            using var pointsNative = new NativeArray<LinePointInfo>(points, Allocator.TempJob);

            using var resultNative = new NativeArray<Vector3>(result, Allocator.TempJob);

            var job = new UpdateMeshJob()
            {
                FromScroll = ScrollManager.Instance.WatchingFrom,
                ToScroll = ScrollManager.Instance.WatchingTo,
                ScrollAmounts = scrollAmountsNative,
                Points = pointsNative,
                Vertics = resultNative
            };
            job.Run(length);
            resultNative.CopyTo(result);
        }
    }
}
