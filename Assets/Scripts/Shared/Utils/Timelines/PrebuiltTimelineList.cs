using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Maths;

namespace Utils.Timelines
{
    public sealed class FloatTimelineList : TimelineList<float>
    {
        public override float GetValueFromTime(float time)
        {
            return FloatTimelineJob.Evaluate(time, JobDatas);
        }

        public override bool IsEqual(in float a, in float b)
        {
            return a == b;
        }
    }

    public sealed class PolarPointTimelineList : TimelineList<PolarPoint>
    {
        public override PolarPoint GetValueFromTime(float time)
        {
            return PolarPointTimelineJob.Evaluate(time, JobDatas);
        }

        public override bool IsEqual(in PolarPoint a, in PolarPoint b)
        {
            return a == b;
        }
    }

    public sealed class Vector2TimelineList : TimelineList<Vector2>
    {
        public override Vector2 GetValueFromTime(float time)
        {
            return Vector2TimelineJob.Evaluate(time, JobDatas);
        }

        public override bool IsEqual(in Vector2 a, in Vector2 b)
        {
            return a == b;
        }
    }

    public sealed class Vector3TimelineList : TimelineList<Vector3>
    {
        public override Vector3 GetValueFromTime(float time)
        {
            return Vector3TimelineJob.Evaluate(time, JobDatas);
        }

        public override bool IsEqual(in Vector3 a, in Vector3 b)
        {
            return a == b;
        }
    }
}
