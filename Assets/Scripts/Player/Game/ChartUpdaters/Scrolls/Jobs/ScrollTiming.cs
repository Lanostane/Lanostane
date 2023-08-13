using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Maths;

namespace LST.Player.Scrolls
{
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
