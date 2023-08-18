using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Scrolls
{
    public enum OutScrollType : byte
    {
        Visible,
        Early,
        Late
    }

    public struct ScrollData
    {
        public float Timing;
        public float Speed;
        public float Duration;

        public readonly float GetPassedTime(float time)
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

    public struct ScrollProgress
    {
        public Millisecond Timing;
        public float Progress;
        public float EasedProgress;
        public OutScrollType VisibleStatus;

        public readonly bool IsVisible => VisibleStatus == OutScrollType.Visible;

        public static ScrollProgress Create(Millisecond from, Millisecond to, Millisecond timing, GameSpaceEaseMode easeMode = GameSpaceEaseMode.Default)
        {
            var uneasedP = Millisecond.InverseLerp(to, from, timing);
            var outscrollType = OutScrollType.Visible;
            var inScreen = true;
            if (timing > to)
            {
                inScreen = false;
                outscrollType = OutScrollType.Early;
            }
            else if (timing < from)
            {
                inScreen = false;
                outscrollType = OutScrollType.Late;
            }

            return new()
            {
                Timing = timing,
                VisibleStatus = outscrollType,
                Progress = uneasedP,
                EasedProgress = inScreen ? Ease.GameSpaceEase(uneasedP, easeMode) : uneasedP
            };
        }
    }

    public readonly struct ScrollTiming
    {
        public readonly Millisecond Timing;
        public readonly ushort ScrollGroupID;

        public ScrollTiming(ushort scrollGroupID, Millisecond scrollTiming)
        {
            Timing = scrollTiming;
            ScrollGroupID = scrollGroupID;
        }
    }
}
