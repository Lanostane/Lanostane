using Lanostane.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.Collections;
using UnityEngine;
using Utils;
using Utils.Maths;

namespace LST.Player.Scrolls
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

    public struct ScrollRangeData
    {
        public Millisecond From;
        public Millisecond To;
    }

    public class ScrollUpdater : MonoBehaviour, IScrollUpdater
    {
        [field: SerializeField]
        [field: Range(1.0f, 9.0f)]
        public float ScrollingSpeed { get; set; } = 7.5f;

        private readonly ScrollGroup _MainScrolls = new(groupID: 0);
        private readonly Dictionary<ushort, ScrollGroup> _ScrollGroups = new();
        private NativeHashMap<ushort, ScrollRangeData> _ScrollRangesNativeData;

        void Awake()
        {
            GamePlay.ScrollUpdater = this;
        }

        void OnDestroy()
        {
            GamePlay.ScrollUpdater = null;
            if (_ScrollRangesNativeData.IsCreated)
            {
                _ScrollRangesNativeData.Dispose();
            }
        }

        public void TimeUpdate(float chartTime)
        {
            var speed = ScrollingSpeed;
            _MainScrolls.UpdateByChartTime(speed, chartTime);
            _ScrollRangesNativeData[_MainScrolls.GroupID] = _MainScrolls.RangeData;
            foreach (var group in _ScrollGroups.Values)
            {
                group.UpdateByChartTime(speed, chartTime);
                _ScrollRangesNativeData[group.GroupID] = group.RangeData;
            }
        }

        public void CleanUp()
        {
            _MainScrolls.Scrolls.Clear();
            foreach (var group in _ScrollGroups.Values)
            {
                group.Scrolls.Clear();
            }
            _ScrollGroups.Clear();
            if (_ScrollRangesNativeData.IsCreated)
            {
                _ScrollRangesNativeData.Dispose();
            }
        }

        public void AddFromChart(LST_Chart chart)
        {
            foreach (var scroll in chart.Scrolls)
            {
                AddScroll(scroll);
            }
        }

        public void AddScroll(LST_ScrollChange scrollChange)
        {
            if (scrollChange.Group == 0)
            {
                _MainScrolls.Scrolls.Add(new()
                {
                    Timing = scrollChange.Timing,
                    Speed = scrollChange.Speed
                });
            }
            else if (_ScrollGroups.TryGetValue(scrollChange.Group, out var scrollGroup))
            {
                scrollGroup.Scrolls.Add(new()
                {
                    Timing = scrollChange.Timing,
                    Speed = scrollChange.Speed
                });
            }
            else
            {
                var group = _ScrollGroups[scrollChange.Group] = new(scrollChange.Group);
                group.Scrolls.Add(new()
                {
                    Timing = scrollChange.Timing,
                    Speed = scrollChange.Speed
                });
            }
        }

        public void Prepare()
        {
            _MainScrolls.SortItems();
            foreach (var group in _ScrollGroups.Values)
            {
                group.SortItems();
            }

            if (_ScrollRangesNativeData.IsCreated)
            {
                _ScrollRangesNativeData.Dispose();
            }
            _ScrollRangesNativeData = new(1 + _ScrollGroups.Count, Allocator.Persistent);
        }

        public bool TryGetGroup(ushort groupID, out ScrollGroup group)
        {
            if (groupID == 0)
            {
                group = _MainScrolls;
                return true;
            }

            return _ScrollGroups.TryGetValue(groupID, out group);
        }

        public Millisecond GetScrollTimingByTime(ushort scrollGroupID, float time)
        {
            if (TryGetGroup(scrollGroupID, out var group))
            {
                return group.GetScrollTimingByTime(time);
            }

            throw new KeyNotFoundException($"{scrollGroupID} is not a valid scroll group ID");
        }

        public float GetProgressionSingle(ushort scrollGroupID, float chartTime, float timing, out bool isInScreen)
        {
            if (TryGetGroup(scrollGroupID, out var group))
            {
                return group.GetProgressionSingle(chartTime, timing, out isInScreen);
            }

            throw new KeyNotFoundException($"{scrollGroupID} is not a valid scroll group ID");
        }

        public float GetProgressionSingleFast(ushort scrollGroupID, Millisecond scrollTiming, out bool isInScreen)
        {
            if (TryGetGroup(scrollGroupID, out var group))
            {
                return group.GetProgressionSingleFast(scrollTiming, out isInScreen);
            }

            throw new KeyNotFoundException($"{scrollGroupID} is not a valid scroll group ID");
        }

        public bool IsScrollRangeVisible(ushort scrollGroupID, Millisecond minAmount, Millisecond maxAmount)
        {
            if (TryGetGroup(scrollGroupID, out var group))
            {
                return group.IsScrollRangeVisible(minAmount, maxAmount);
            }

            throw new KeyNotFoundException($"{scrollGroupID} is not a valid scroll group ID");
        }

        public ScrollProgress[] GetProgressions(ushort scrollGroupID, float chartTime, float[] timings)
        {
            if (TryGetGroup(scrollGroupID, out var group))
            {
                return group.GetProgressions(chartTime, timings);
            }

            throw new KeyNotFoundException($"{scrollGroupID} is not a valid scroll group ID");
        }

        public void GetNativeScrollRangeData(ref NativeHashMap<ushort, ScrollRangeData> hashMap)
        {
            hashMap = _ScrollRangesNativeData;
        }
    }
}
