using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using Utils.Maths;

namespace LST.Player.Scrolls
{
    public sealed class ScrollGroup
    {
        public readonly ushort GroupID;
        public readonly FastList<ScrollData> Scrolls = new();
        public ScrollRangeData RangeData { get; private set; }

        public Millisecond WatchingFrom { get; private set; }
        public Millisecond WatchingTo { get; private set; }
        public float EndAmountFactor { get; private set; }

        public ScrollGroup(ushort groupID)
        {
            GroupID = groupID;
        }

        public void UpdateByChartTime(float scrollSpeed, float chartTime)
        {
            var speedPercentage = Mathf.InverseLerp(1.0f, 9.0f, scrollSpeed);
            EndAmountFactor = Mathf.Lerp(4.5f, 0.6f, speedPercentage);

            WatchingFrom = Millisecond.Zero;

            var items = Scrolls.Items;
            for (int i = 0; i < items.Length; i++)
            {
                var scroll = items[i];
                if (chartTime >= scroll.Timing)
                {
                    WatchingFrom += new Millisecond(scroll.Speed * scroll.GetPassedTime(chartTime));
                }
            }

            WatchingTo = WatchingFrom + EndAmountFactor;
            RangeData = new()
            {
                From = WatchingFrom,
                To = WatchingTo
            };
        }

        public void SortItems()
        {
            var sorted = Scrolls.Items.OrderBy(x => x.Timing).ToArray();
            for (int i = 0; i < sorted.Length; i++)
            {
                var scroll = sorted[i];
                scroll.Duration = float.MaxValue;

                if (i > 0)
                {
                    var prevScroll = sorted[i - 1];
                    prevScroll.Duration = scroll.Timing - prevScroll.Timing;
                    sorted[i - 1] = prevScroll;
                }

                sorted[i] = scroll;
            }

            Scrolls.Clear();
            Scrolls.AddRange(sorted);
        }

        public Millisecond GetScrollTimingByTime(float time)
        {
            Millisecond timingScrollAmount = Millisecond.Zero;

            var items = Scrolls.Items;
            for (int i = 0; i < items.Length; i++)
            {
                var scroll = items[i];
                if (time >= scroll.Timing)
                {
                    timingScrollAmount += new Millisecond(scroll.Speed * scroll.GetPassedTime(time));
                }
            }
            return timingScrollAmount;
        }

        public float GetProgressionSingle(float chartTime, float timing, out bool isInScreen)
        {
            var chartScrollAmount = Millisecond.Zero;
            var timingScrollAmount = Millisecond.Zero;

            var items = Scrolls.Items;
            for (int i = 0; i < items.Length; i++)
            {
                var scroll = items[i];
                if (chartTime >= scroll.Timing)
                {
                    chartScrollAmount += new Millisecond(scroll.Speed * scroll.GetPassedTime(chartTime));
                }

                if (timing >= scroll.Timing)
                {
                    timingScrollAmount += scroll.Speed * scroll.GetPassedTime(timing);
                }
            }

            isInScreen = true;
            return Millisecond.InverseLerp(chartScrollAmount + EndAmountFactor, chartScrollAmount, timingScrollAmount);
        }

        public float GetProgressionSingleFast(Millisecond scrollTiming, out bool isInScreen)
        {
            if (WatchingFrom <= scrollTiming && scrollTiming <= WatchingTo)
            {
                isInScreen = true;
            }
            else
            {
                isInScreen = false;
            }

            return Millisecond.InverseLerp(WatchingTo, WatchingFrom, scrollTiming);
        }

        public bool IsScrollRangeVisible(Millisecond minAmount, Millisecond maxAmount)
        {
            var from = WatchingFrom;
            var to = WatchingTo;
            if (WithIn(from, to, minAmount))
            {
                return true;
            }
            else if (WithIn(from, to, maxAmount))
            {
                return true;
            }
            else
            {
                if (maxAmount < from) //Fully Outside of Border (Left)
                    return false;

                if (minAmount > to) //Fully Outside of Border (Right)
                    return false;

                if (minAmount <= from && to <= maxAmount) //Min max is both larger than from to
                    return true;
            }

            return false;

            static bool WithIn(Millisecond min, Millisecond max, Millisecond p)
            {
                return min <= p && p <= max;
            }
        }

        public ScrollProgress[] GetProgressions(float chartTime, float[] timings)
        {
            if (timings == null)
                return Array.Empty<ScrollProgress>();

            var from = GetScrollTimingByTime(chartTime);
            var to = from + EndAmountFactor;
            var list = TempList<ScrollProgress>.GetList();
            foreach (var timing in timings)
            {
                list.Add(ScrollProgress.Create(from, to, GetScrollTimingByTime(timing)));
            }
            return list.ToArray();
        }
    }
}
