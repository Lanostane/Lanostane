using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Maths;

namespace Utils.Timelines
{
    public struct TimelineItemData<T> where T : struct
    {
        public float Timing;
        public float Duration;
        public T Param;
        public bool IsDeltaMode;
        public EaseType Easing;
    }
}
