using UnityEngine;

namespace Utils.Maths
{
    public enum GameSpaceEaseMode : byte
    {
        Default,
        Linear,
        Deceleration
    }

    public static class Ease
    {
        public static float GameSpaceEase(float t, GameSpaceEaseMode easeMode = GameSpaceEaseMode.Default)
        {
            if (easeMode == GameSpaceEaseMode.Default)
            {
                t = Mathf.Clamp01(t);
                t = Cubic.In(t);
                return BezierLikeEasing(t, 0.15f, 0.10f);
            }
            else if (easeMode == GameSpaceEaseMode.Deceleration)
            {
                t = Mathf.Clamp01(t);
                t = Cubic.In(t);
                return BezierLikeEasing(t, -0.24f, 1.0f);
            }
            else //Linear Or Default
            {
                t = Mathf.Clamp01(t);
                t = Cubic.In(t);
                return t;
            }
        }

        //Visualize: https://www.desmos.com/calculator/eqkqcnogko
        private static float BezierLikeEasing(float t, float p1, float p2)
        {
            float pr1 = 3.0f * Mathf.Pow(1 - t, 2.0f) * t * p1;
            float pr2 = 3.0f * Mathf.Pow(t, 2.0f) * (1.0f - t) * p2;
            float pr3 = Mathf.Pow(t, 3.0f);
            return pr1 + pr2 + pr3;
        }

        public static float Linear(float k)
        {
            return k;
        }

        public class Quadratic
        {
            public static float In(float k)
            {
                return k * k;
            }

            public static float Out(float k)
            {
                return k * (2f - k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k;
                return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
            }
        };

        public class Cubic
        {
            public static float In(float k)
            {
                return k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k;
                return 0.5f * ((k -= 2f) * k * k + 2f);
            }
        };

        public class Quartic
        {
            public static float In(float k)
            {
                return k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f - ((k -= 1f) * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
                return -0.5f * ((k -= 2f) * k * k * k - 2f);
            }
        };

        public class Quintic
        {
            public static float In(float k)
            {
                return k * k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
                return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
            }
        };

        public class Sinusoidal
        {
            public static float In(float k)
            {
                return 1f - FastTrig.RadCos(k * MathfE.HalfPI);
            }

            public static float Out(float k)
            {
                return FastTrig.RadSin(k * MathfE.HalfPI);
            }

            public static float InOut(float k)
            {
                return 0.5f * (1f - FastTrig.RadCos(Mathf.PI * k));
            }
        };

        public class Exponential
        {
            public static float In(float k)
            {
                return k == 0f ? 0f : Mathf.Pow(1024f, k - 1f);
            }

            public static float Out(float k)
            {
                return k == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * k);
            }

            public static float InOut(float k)
            {
                if (k == 0f) return 0f;
                if (k == 1f) return 1f;
                if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
                return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
            }
        };

        public class Circular
        {
            public static float In(float k)
            {
                return 1f - Mathf.Sqrt(1f - k * k);
            }

            public static float Out(float k)
            {
                return Mathf.Sqrt(1f - ((k -= 1f) * k));
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (Mathf.Sqrt(1f - k * k) - 1);
                return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
            }
        };

        public class Elastic
        {
            public static float In(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return -Mathf.Pow(2f, 10f * (k -= 1f)) * FastTrig.RadSin((k - 0.1f) * (MathfE.TwoPI) / 0.4f);
            }

            public static float Out(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return Mathf.Pow(2f, -10f * k) * FastTrig.RadSin((k - 0.1f) * (MathfE.TwoPI) / 0.4f) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * FastTrig.RadSin((k - 0.1f) * (MathfE.TwoPI) / 0.4f);
                return Mathf.Pow(2f, -10f * (k -= 1f)) * FastTrig.RadSin((k - 0.1f) * (MathfE.TwoPI) / 0.4f) * 0.5f + 1f;
            }
        };

        public class Back
        {
            public const float C1 = 1.70158f;
            public const float C2 = 2.5949095f;

            public static float In(float k)
            {
                return k * k * ((C1 + 1f) * k - C1);
            }

            public static float Out(float k)
            {
                return (k -= 1f) * k * ((C1 + 1f) * k + C1) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * (k * k * ((C2 + 1f) * k - C2));
                return 0.5f * ((k -= 2f) * k * ((C2 + 1f) * k + C2) + 2f);
            }
        };

        public class Bounce
        {
            public static float In(float k)
            {
                return 1f - Out(1f - k);
            }

            public static float Out(float k)
            {
                if (k < (1f / 2.75f))
                {
                    return 7.5625f * k * k;
                }
                else if (k < (2f / 2.75f))
                {
                    return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
                }
                else if (k < (2.5f / 2.75f))
                {
                    return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
                }
                else
                {
                    return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
                }
            }

            public static float InOut(float k)
            {
                if (k < 0.5f) return In(k * 2f) * 0.5f;
                return Out(k * 2f - 1f) * 0.5f + 0.5f;
            }
        };
    }
}
