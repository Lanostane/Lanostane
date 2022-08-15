using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Maths
{
    public struct Millisecond : IComparable<Millisecond>, IComparer<Millisecond>, IEqualityComparer<Millisecond>
    {
        public const float TimeConversion = 1.0f / 1000.0f;
        public const double DoubleTimeConversion = 1.0 / 1000.0;

        public static readonly Millisecond Zero = new(0.0f);

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

        public Millisecond(float time)
        {
            _MS = ToMS(time);
        }

        public Millisecond(long ms)
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

        public static Millisecond Lerp(Millisecond a, Millisecond b, float t)
        {
            return a + (long)((double)((b - a).Milisecond) * Mathf.Clamp01(t));
        }

        public static float InverseLerp(Millisecond a, Millisecond b, Millisecond p)
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

        public int Compare(Millisecond x, Millisecond y)
        {
            return x.Milisecond.CompareTo(y);
        }

        public bool Equals(Millisecond x, Millisecond y)
        {
            return x.Milisecond == y.Milisecond;
        }

        public int GetHashCode(Millisecond obj)
        {
            return obj.Milisecond.GetHashCode();
        }

        public int CompareTo(Millisecond other)
        {
            return _MS.CompareTo(other.Milisecond);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _MS.GetHashCode();
        }

        public static Millisecond operator +(Millisecond a) => a;
        public static Millisecond operator -(Millisecond a) => new(-a.Milisecond);
        public static Millisecond operator +(Millisecond a, Millisecond b) => new(a.Milisecond + b.Milisecond);
        public static Millisecond operator -(Millisecond a, Millisecond b) => new(a.Milisecond - b.Milisecond);
        public static Millisecond operator +(Millisecond a, float time) => new(a.Time + time);
        public static Millisecond operator -(Millisecond a, float time) => new(a.Time - time);

        public static bool operator <(Millisecond a, Millisecond b) => a.Milisecond < b.Milisecond;
        public static bool operator >(Millisecond a, Millisecond b) => a.Milisecond > b.Milisecond;
        public static bool operator <=(Millisecond a, Millisecond b) => a.Milisecond <= b.Milisecond;
        public static bool operator >=(Millisecond a, Millisecond b) => a.Milisecond >= b.Milisecond;
        public static bool operator !=(Millisecond a, Millisecond b) => a.Milisecond != b.Milisecond;
        public static bool operator ==(Millisecond a, Millisecond b) => a.Milisecond == b.Milisecond;
    }
}
