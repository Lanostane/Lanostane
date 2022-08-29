using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GamePlay.Graphics.FX.Hold
{
    [BurstCompile]
    public struct CreateMeshJob : IJobParallelFor
    {
        public int PointsLength;
        public float UVTimeOffset;
        [NativeDisableParallelForRestriction]
        [ReadOnly] public NativeArray<LinePointInfo> Points;

        [NativeDisableParallelForRestriction]
        [WriteOnly] public NativeArray<Vector3> Vertics;
        [NativeDisableParallelForRestriction]
        [WriteOnly] public NativeArray<Vector2> UVs;
        [NativeDisableParallelForRestriction]
        [WriteOnly] public NativeArray<int> Trigs;

        public void Execute(int index)
        {
            var leftIndex = index * 2;
            var rightIndex = (index * 2) + 1;
            var topLeftIndex = (index * 2) + 2;
            var topRightIndex = (index * 2) + 3;

            var point = Points[index];

            var uvY = Mathf.Repeat((point.Timing - UVTimeOffset) * 0.25f, 1.0f);

            Vertics[leftIndex] = Vector3.zero;
            Vertics[rightIndex] = Vector3.zero;
            UVs[leftIndex] = new Vector2(0.0f, uvY);
            UVs[rightIndex] = new Vector2(1.0f, uvY);

            if (index < PointsLength - 1) //Skipping End Trig Build
            {
                var p0 = leftIndex;
                var p1 = rightIndex;
                var p2 = topLeftIndex;
                var p3 = topRightIndex;

                var ta1Idx = index * 6;
                var ta2Idx = (index * 6) + 1;
                var ta3Idx = (index * 6) + 2;
                var tb1Idx = (index * 6) + 3;
                var tb2Idx = (index * 6) + 4;
                var tb3Idx = (index * 6) + 5;

                //    p2                 p3
                //    |          /       |
                //    |       /          |
                //    p0                 p1
                //
                // Trig 1: p3 -> p0 -> p1
                // Trig 2: p0 -> p3 -> p2

                //Trig 1
                Trigs[ta1Idx] = p3;
                Trigs[ta2Idx] = p0;
                Trigs[ta3Idx] = p1;

                //Trig 2
                Trigs[tb1Idx] = p0;
                Trigs[tb2Idx] = p3;
                Trigs[tb3Idx] = p2;
            }
        }

        public static void Create(float startTime, LinePointInfo[] points, Mesh mesh)
        {
            var length = points.Length;
            var verticsLength = length * 2;
            using var pointsNative = new NativeArray<LinePointInfo>(points, Allocator.TempJob);

            using var verticsNative = new NativeArray<Vector3>(new Vector3[verticsLength], Allocator.TempJob);
            using var uvsNative = new NativeArray<Vector2>(new Vector2[verticsLength], Allocator.TempJob);
            using var trigsNative = new NativeArray<int>(new int[(verticsLength - 2) * 3], Allocator.TempJob);

            var job = new CreateMeshJob()
            {
                UVTimeOffset = startTime,
                PointsLength = length,
                Points = pointsNative,
                Vertics = verticsNative,
                UVs = uvsNative,
                Trigs = trigsNative
            };

            job.Run(length);

            mesh.SetVertices(verticsNative.ToArray());
            mesh.SetUVs(0, uvsNative.ToArray());
            mesh.SetTriangles(trigsNative.ToArray(), 0);
        }
    }
}
