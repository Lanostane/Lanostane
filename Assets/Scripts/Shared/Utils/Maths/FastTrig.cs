using UnityEngine;

namespace Utils.Maths
{
    public static class FastTrig
    {
        public static float Sin(float degree)
        {
            return UnsafeSin(MathfE.AbsAngle(degree) * Mathf.Deg2Rad);
        }

        public static float Cos(float degree)
        {
            return UnsafeCos(MathfE.AbsAngle(degree) * Mathf.Deg2Rad);
        }

        public static float UnsafeSin(float rad)
        {
            float sinn;
            if (rad < -3.14159265f)
                rad += 6.28318531f;
            else
            if (rad > 3.14159265f)
                rad -= 6.28318531f;

            if (rad < 0)
            {
                sinn = 1.27323954f * rad + 0.405284735f * rad * rad;

                if (sinn < 0)
                    sinn = 0.225f * (sinn * -sinn - sinn) + sinn;
                else
                    sinn = 0.225f * (sinn * sinn - sinn) + sinn;
                return sinn;
            }
            else
            {
                sinn = 1.27323954f * rad - 0.405284735f * rad * rad;

                if (sinn < 0)
                    sinn = 0.225f * (sinn * -sinn - sinn) + sinn;
                else
                    sinn = 0.225f * (sinn * sinn - sinn) + sinn;
                return sinn;

            }
        }

        public static float UnsafeCos(float rad)
        {
            return UnsafeSin(rad + 1.5707963f);
        }
    }
}
