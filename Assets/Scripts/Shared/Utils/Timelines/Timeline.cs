using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Utils.Maths;

namespace Utils.Timelines
{
    public sealed class FloatTimeline
    {
        public readonly FastList<FloatTimelineItem> Items = new();
        public float Value { get; private set; } = 0.0f;

        public event Action<float> ValueUpdated;

        public void TimeUpdated(float time)
        {
            var v = FloatTimelineJob.Evaluate(time, Items.Items);
            if (v != Value)
            {
                Value = v;
                ValueUpdated?.Invoke(v);
            }
        }
    }

    public sealed class PolarPointTimeline
    {
        public readonly FastList<PolarPointTimelineItem> Items = new();
        public PolarPoint Value { get; private set; } = new PolarPoint(0.0f, 0.0f);

        public event Action<PolarPoint> ValueUpdated;

        public void TimeUpdated(float time)
        {
            var v = PolarPointTimelineJob.Evaluate(time, Items.Items);
            if (v != Value)
            {
                Value = v;
                ValueUpdated?.Invoke(v);
            }
        }
    }

    public sealed class Vector2Timeline
    {
        public readonly FastList<Vector2TimelineItem> Items = new();
        public Vector2 Value { get; private set; } = Vector2.zero;

        public event Action<Vector2> ValueUpdated;

        public void TimeUpdated(float time)
        {
            var v = Vector2TimelineJob.Evaluate(time, Items.Items);
            if (v != Value)
            {
                Value = v;
                ValueUpdated?.Invoke(v);
            }
        }
    }

    public sealed class Vector3Timeline
    {
        public readonly FastSortedList<Vector3TimelineItem> Items = new(x => x.Timing, SortBy.AscendingOrder);
        public Vector3 Value { get; private set; } = Vector3.zero;

        public event Action<Vector3> ValueUpdated;

        public void TimeUpdated(float time)
        {
            var v = Vector3TimelineJob.Evaluate(time, Items.Items);
            if (v != Value)
            {
                Value = v;
                ValueUpdated?.Invoke(v);
            }
        }
    }
}
