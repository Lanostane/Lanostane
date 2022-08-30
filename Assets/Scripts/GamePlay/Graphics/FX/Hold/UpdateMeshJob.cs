using GamePlay.Scrolls;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Utils.Maths;

namespace GamePlay.Graphics.FX.Hold
{
    [BurstCompile]
    public struct UpdateMeshJob : IJobParallelFor
    {
        public const float SIZE = 4.15f;

        public Millisecond FromScroll;
        public Millisecond ToScroll;

        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<Millisecond> ScrollAmounts;
        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<LinePointInfo> Points;
        [NativeDisableParallelForRestriction]
        [WriteOnly] public NativeArray<Vector3> Vertics;

        public void Execute(int index)
        {
            var amount = ScrollAmounts[index];
            var point = Points[index];
            var progress = Millisecond.InverseLerp(ToScroll, FromScroll, amount);
            if (progress > 0.0f && progress < 1.0f)
            {
                progress = Ease.GameSpaceEase(progress);
            }

            var radius = GameConst.LerpSpace(progress);

            Vertics[index * 2] = point.LeftDir * radius;
            Vertics[(index * 2) + 1] = point.RightDir * radius;
        }

        public static void UpdateVertics(Millisecond[] amounts, LinePointInfo[] points, Vector3[] result)
        {
            var length = amounts.Length;

            using var scrollAmountsNative = new NativeArray<Millisecond>(amounts, Allocator.TempJob);
            using var pointsNative = new NativeArray<LinePointInfo>(points, Allocator.TempJob);

            using var resultNative = new NativeArray<Vector3>(result, Allocator.TempJob);

            var job = new UpdateMeshJob()
            {
                FromScroll = ScrollUpdater.Instance.WatchingFrom,
                ToScroll = ScrollUpdater.Instance.WatchingTo,
                ScrollAmounts = scrollAmountsNative,
                Points = pointsNative,
                Vertics = resultNative
            };
            job.Run(length);
            resultNative.CopyTo(result);
        }
    }
}
