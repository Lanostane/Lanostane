using System.Collections;
using TMPro;
using UnityEngine;
using Utils.Maths;

namespace LST.GamePlay.Scrolls
{
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

    public enum OutScrollType : byte
    {
        Visible,
        Early,
        Late
    }
}