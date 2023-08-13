using LST.Player.Scrolls;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Utils.Maths;

namespace LST.Player.Graphics
{
    [BurstCompile]
    public struct UpdateMeshJob : IJobParallelFor
    {
        public const float SIZE = 4.15f;

        public Millisecond FromScroll;
        public Millisecond ToScroll;
        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<Millisecond> ScrollTimings;
        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<LinePointInfo> Points;
        [NativeDisableParallelForRestriction]
        [WriteOnly] public NativeArray<Vector3> Vertics;
        

        public void Execute(int index)
        {
            var timing = ScrollTimings[index];
            var point = Points[index];
            var progress = Millisecond.InverseLerp(ToScroll, FromScroll, timing);
            if (progress > 0.0f && progress < 1.0f)
            {
                progress = Ease.GameSpaceEase(progress);
            }

            var radius = GameConst.LerpSpaceFactor(progress);

            Vertics[index * 2] = point.LeftDir * radius;
            Vertics[(index * 2) + 1] = point.RightDir * radius;
        }

        public static void UpdateVertics(ushort scrollGroupID, Millisecond[] amounts, LinePointInfo[] points, Vector3[] result)
        {
            if (!GamePlayManager.ScrollUpdater.TryGetGroup(scrollGroupID, out var group))
            {
                return;
            }

            var length = amounts.Length;

            using var scrollTimingsNative = new NativeArray<Millisecond>(amounts, Allocator.TempJob);
            using var pointsNative = new NativeArray<LinePointInfo>(points, Allocator.TempJob);

            using var resultNative = new NativeArray<Vector3>(result, Allocator.TempJob);

            var job = new UpdateMeshJob()
            {
                FromScroll = group.WatchingFrom,
                ToScroll = group.WatchingTo,
                ScrollTimings = scrollTimingsNative,
                Points = pointsNative,
                Vertics = resultNative
            };
            job.Schedule(length, 8).Complete();
            resultNative.CopyTo(result);
        }
    }
}
