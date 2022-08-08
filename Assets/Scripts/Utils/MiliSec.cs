using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public struct MiliSec : IComparable<MiliSec>, IComparer<MiliSec>, IEqualityComparer<MiliSec>
    {
        public const float TimeConversion = 1.0f / 1000.0f;
        public const double DoubleTimeConversion = 1.0 / 1000.0;

        public static readonly MiliSec Zero = new(0.0f);

        public long Milisecond
        {
            get => _MS;
            set => _MS = value;
        }

        public float Time
        {
            get => _MS * TimeConversion;
            set => _MS = ToMS(value);
        }

        public double DoubleTime
        {
            get => _MS * DoubleTimeConversion;
            set => _MS = ToMS(value);
        }

        private long _MS;

        public MiliSec(float time)
        {
            _MS = ToMS(time);
        }

        public MiliSec(long ms)
        {
            _MS = ms;
        }

        public static long ToMS(float time)
        {
            return (long)(time * 1000.0f);
        }

        public static long ToMS(double time)
        {
            return (long)(time * 1000.0);
        }

        public static MiliSec Lerp(MiliSec a, MiliSec b, float t)
        {
            return a + (long)((double)((b - a).Milisecond) * Mathf.Clamp01(t));
        }

        public static float InverseLerp(MiliSec a, MiliSec b, MiliSec p)
        {
            var aTime = a.Milisecond;
            var bTime = b.Milisecond;
            var pTime = p.Milisecond;

            if (aTime == bTime)
                return 0.0f;

            if (aTime > bTime)
            {
                return 1.0f - InverseLerp_Unsafe(bTime, aTime, pTime);
            }
            else
            {
                return InverseLerp_Unsafe(bTime, aTime, pTime);
            }
        }

        private static float InverseLerp_Unsafe(long min, long max, long t)
        {
            if (t <= min)
                return 0.0f;

            if (t >= max)
                return 1.0f;

            max -= min;
            t -= min;
            return (float)t / max;
        }

        public int Compare(MiliSec x, MiliSec y)
        {
            return x.Milisecond.CompareTo(y);
        }

        public bool Equals(MiliSec x, MiliSec y)
        {
            return x.Milisecond == y.Milisecond;
        }

        public int GetHashCode(MiliSec obj)
        {
            return obj.Milisecond.GetHashCode();
        }

        public int CompareTo(MiliSec other)
        {
            return _MS.CompareTo(other.Milisecond);
        }

        public static MiliSec operator +(MiliSec a) => a;
        public static MiliSec operator -(MiliSec a) => new(-a.Milisecond);
        public static MiliSec operator +(MiliSec a, MiliSec b) => new(a.Milisecond + b.Milisecond);
        public static MiliSec operator -(MiliSec a, MiliSec b) => new(a.Milisecond - b.Milisecond);
        public static MiliSec operator +(MiliSec a, float time) => new(a.Time + time);
        public static MiliSec operator -(MiliSec a, float time) => new(a.Time - time);

        public static bool operator <(MiliSec a, MiliSec b) => a.Milisecond < b.Milisecond;
        public static bool operator >(MiliSec a, MiliSec b) => a.Milisecond > b.Milisecond;
        public static bool operator <=(MiliSec a, MiliSec b) => a.Milisecond <= b.Milisecond;
        public static bool operator >=(MiliSec a, MiliSec b) => a.Milisecond >= b.Milisecond;
        public static bool operator !=(MiliSec a, MiliSec b) => a.Milisecond != b.Milisecond;
        public static bool operator ==(MiliSec a, MiliSec b) => a.Milisecond == b.Milisecond;
    }
}
