using Lst.Charts;
using Lst.GamePlay.Scrolls.Jobs;
using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;
using Utils.Maths;

namespace Lst.GamePlay.Scrolls
{
    public struct ScrollData
    {
        public float Timing;
        public float Speed;
        public float Duration;

        public float GetPassedTime(float time)
        {
            if (time <= Timing)
                return 0.0f;

            return Mathf.Min(time - Timing, Duration);
        }
    }

    public interface IScrollManager : IChartUpdater
    {
        Millisecond WatchingFrom { get; }
        Millisecond WatchingTo { get; }
        void AddScroll(LST_ScrollChange scrollChange);
        void UpdateAbsValue();
        Millisecond GetScrollTimingByTime(float time);
        float GetProgressionSingle(float chartTime, float timing, out bool isInScreen);
        float GetProgressionSingleFast(Millisecond scrollTiming, out bool isInScreen);
        ScrollAmountInfo[] GetProgressions(float chartTime, float[] timings);
    }

    public class ScrollManager : MonoBehaviour, IScrollManager
    {
        public static IScrollManager Instance { get; private set; }

        [Range(1.0f, 9.0f)]
        public float Speed = 7.5f;
        public Millisecond WatchingFrom { get; private set; }
        public Millisecond WatchingTo { get; private set; }

        private readonly FastList<ScrollData> _Scrolls = new();
        public ScrollData[] ScrollDatas => _Scrolls.Items;

        private float _EndAmountFactor = 1.35f;

        void Awake()
        {
            Instance = this;
        }

        void OnDestroy()
        {
            Instance = null;
        }

        void OnValidate()
        {
            var speedP = Mathf.InverseLerp(1.0f, 9.0f, Speed);
            _EndAmountFactor = Mathf.Lerp(4.5f, 0.6f, speedP);
        }

        public void UpdateChart(float chartTime)
        {
            WatchingFrom = Millisecond.Zero;

            var items = _Scrolls.Items;
            for(int i = 0; i<items.Length; i++)
            {
                var scroll = items[i];
                if (chartTime >= scroll.Timing)
                {
                    WatchingFrom += new Millisecond(scroll.Speed * scroll.GetPassedTime(chartTime));
                }
            }

            WatchingTo = WatchingFrom + _EndAmountFactor;
        }

        public void CleanUp()
        {
            _Scrolls.Clear();
        }

        public void AddScroll(LST_ScrollChange scrollChange)
        {
            _Scrolls.Add(new()
            {
                Timing = scrollChange.Timing,
                Speed = scrollChange.Speed
            });
        }

        public void UpdateAbsValue()
        {
            var sorted = _Scrolls.Items.OrderBy(x => x.Timing).ToArray();
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

            _Scrolls.Clear();
            _Scrolls.AddRange(sorted);
        }

        public Millisecond GetScrollTimingByTime(float time)
        {
            Millisecond timingScrollAmount = Millisecond.Zero;

            var items = _Scrolls.Items;
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

        public float GetProgressionSingle(float chartTime, float timing, out bool isInScreen)
        {
            var chartScrollAmount = Millisecond.Zero;
            var timingScrollAmount = Millisecond.Zero;

            var items = _Scrolls.Items;
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
            return Millisecond.InverseLerp(chartScrollAmount + _EndAmountFactor, chartScrollAmount, timingScrollAmount);
        }

        public static bool IsScrollRangeVisible(Millisecond minAmount, Millisecond maxAmount)
        {
            var from = Instance.WatchingFrom;
            var to = Instance.WatchingTo;
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

        public ScrollAmountInfo[] GetProgressions(float chartTime, float[] timings)
        {
            if (timings == null)
                return Array.Empty<ScrollAmountInfo>();

            var from = GetScrollTimingByTime(chartTime);
            var to = from + _EndAmountFactor;
            return ScrollAmountQueryJob.Run(_Scrolls.Items, from, to, timings);
        }

        public static Millisecond GetScrollTiming(float time)
        {
            return Instance.GetScrollTimingByTime(time);
        }

        public static float GetProgress(float chartTime, float timing, out bool isInScreen)
        {
            return Instance.GetProgressionSingle(chartTime, timing, out isInScreen);
        }

        public static ScrollAmountInfo[] GetProgressBulk(float chartTime, float[] timings)
        {
            return Instance.GetProgressions(chartTime, timings);
        }
    }
}
