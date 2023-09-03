using UnityEngine;

namespace Utils.Maths
{
    public struct PolarPoint
    {
        public float Rho;
        public float Theta;

        public PolarPoint(float rho, float theta)
        {
            Rho = rho;
            Theta = theta;
        }

        public readonly PolarPoint Normalized
        {
            get
            {
                var coord = ToCoord();
                return new PolarPoint(coord.magnitude, Mathf.Atan2(coord.y, coord.x) * Mathf.Rad2Deg);
            }
        }

        public readonly Vector2 ToCoord2D()
        {
            return new Vector2(FastTrig.DegCos(Theta), FastTrig.DegSin(Theta)) * Rho;
        }

        public readonly Vector3 ToCoord()
        {
            return new Vector3(FastTrig.DegCos(Theta), FastTrig.DegSin(Theta), 0.0f) * Rho;
        }

        public static PolarPoint Lerp(PolarPoint p1, PolarPoint p2, float t)
        {
            var theta = Mathf.Lerp(p1.Theta, p2.Theta, t);
            var rho = Mathf.Lerp(p1.Rho, p2.Rho, t);
            return new PolarPoint(rho, theta);
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is PolarPoint other)
            {
                return other == this;
            }
            else
            {
                return false;
            }
        }

        public override readonly int GetHashCode()
        {
            return Rho.GetHashCode() ^ Theta.GetHashCode(); //Lazy
        }

        public override readonly string ToString()
        {
            return $"Rho: {Rho}, Theta: {Theta}";
        }

        public static PolarPoint operator +(PolarPoint a) => a;
        public static PolarPoint operator -(PolarPoint a) => new(-a.Rho, a.Theta);
        public static PolarPoint operator +(PolarPoint a, PolarPoint b) => new(a.Rho + b.Rho, a.Theta + b.Theta);
        public static PolarPoint operator -(PolarPoint a, PolarPoint b) => new(a.Rho - b.Rho, a.Theta - b.Theta);
        public static PolarPoint operator *(PolarPoint p, float multiplier) => new(p.Rho * multiplier, p.Theta * multiplier);

        public static bool operator ==(PolarPoint a, PolarPoint b)
        {
            return (a.Rho == b.Rho) && (a.Theta == b.Theta);
        }

        public static bool operator !=(PolarPoint a, PolarPoint b)
        {
            return (a.Rho != b.Rho) || (a.Theta != b.Theta);
        }
    }
}
