using UnityEngine;

namespace Assets.Scripts.Utils
{
    public static class Easing
    {
        public static class Linear
        {
            public static float easeIn(float t)
            {
                return t;
            }


            public static float easeOut(float t)
            {
                return t;
            }


            public static float easeInOut(float t)
            {
                return t;
            }
        }

        public static class Cubic
        {
            public static float easeIn(float t)
            {
                return Mathf.Pow(t, 3.0f);
            }


            public static float easeOut(float t)
            {
                return (Mathf.Pow(t - 1, 3) - 1)*-1;
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return Quartic.easeIn(t*2)/2;
                return (Quartic.easeOut((t - 0.5f)*2.0f)/2) + 0.5f;
            }
        }


        public static class Quartic
        {
            public static float easeIn(float t)
            {
                return Mathf.Pow(t, 4.0f);
            }


            public static float easeOut(float t)
            {
                return (Mathf.Pow(t - 1, 4) - 1)*-1;
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return easeIn(t*2)/2;
                return (easeOut((t - 0.5f)*2.0f)/2) + 0.5f;
            }
        }


        public static class Quintic
        {
            public static float easeIn(float t)
            {
                return Mathf.Pow(t, 5.0f);
            }


            public static float easeOut(float t)
            {
                return (Mathf.Pow(t - 1, 5) + 1);
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return easeIn(t*2)/2;
                return (easeOut((t - 0.5f)*2.0f)/2) + 0.5f;
            }
        }


        public static class Sinusoidal
        {
            public static float easeIn(float t)
            {
                return Mathf.Sin((t - 1)*(Mathf.PI/2)) + 1;
            }


            public static float easeOut(float t)
            {
                return Mathf.Sin(t*(Mathf.PI/2));
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return easeIn(t*2)/2;
                return (easeOut((t - 0.5f)*2.0f)/2) + 0.5f;
            }
        }


        public static class Exponential
        {
            public static float easeIn(float t)
            {
                return Mathf.Pow(2, 10*(t - 1));
            }


            public static float easeOut(float t)
            {
                return 1 - Mathf.Pow(2, -10*t);
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return easeIn(t*2)/2;
                return easeOut(t*2 - 1)/2 + 0.5f;
            }
        }


        public static class Circular
        {
            public static float easeIn(float t)
            {
                return (-1*Mathf.Sqrt(1 - t*t) + 1);
            }


            public static float easeOut(float t)
            {
                return Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return easeIn(t*2)/2;
                return (easeOut((t - 0.5f)*2.0f)/2) + 0.5f;
            }
        }


        public static class Back
        {
            private const float s = 1.70158f;
            private const float s2 = 1.70158f*1.525f;


            public static float easeIn(float t)
            {
                return t*t*((s + 1)*t - 2);
            }


            public static float easeOut(float t)
            {
                t = t - 1;
                return (t*t*((s + 1)*t + s) + 1);
            }


            public static float easeInOut(float t)
            {
                t = t*2;

                if (t < 1)
                {
                    return 0.5f*(t*t*((s2 + 1)*t - s2));
                }
                t -= 2;
                return 0.5f*(t*t*((s2 + 1)*t + s2) + 2);
            }
        }


        public static class Bounce
        {
            private const float b = 0f;
            private const float c = 1f;
            private const float d = 1f;

            public static float easeOut(float t)
            {
                if ((t /= d) < (1/2.75))
                {
                    return c*(7.5625f*t*t) + b;
                }
                if (t < (2/2.75))
                {
                    return c*(7.5625f*(t -= (1.5f/2.75f))*t + .75f) + b;
                }
                if (t < (2.5/2.75))
                {
                    return c*(7.5625f*(t -= (2.25f/2.75f))*t + .9375f) + b;
                }
                return c*(7.5625f*(t -= (2.625f/2.75f))*t + .984375f) + b;
            }

            public static float easeIn(float t)
            {
                return c - easeOut(d - t) + b;
            }

            public static float easeInOut(float t)
            {
                if (t < d/2) return easeIn(t*2)*0.5f + b;
                return easeOut(t*2 - d)*.5f + c*0.5f + b;
            }
        }


        public static class Elastic
        {
            private const float p = 0.3f;
            private static readonly float a = 1;


            private static float calc(float t, bool easingIn)
            {
                if (t == 0 || t == 1)
                    return t;

                float s;

                if (a < 1)
                    s = p/4;
                else
                    s = p/(2*Mathf.PI)*Mathf.Asin(1/a);

                if (easingIn)
                {
                    t -= 1;
                    return -(a*Mathf.Pow(2, 10*t))*Mathf.Sin((t - s)*(2*Mathf.PI)/p);
                }
                return a*Mathf.Pow(2, -10*t)*Mathf.Sin((t - s)*(2*Mathf.PI)/p) + 1;
            }


            public static float easeIn(float t)
            {
                return 1 - easeOut(1 - t);
            }


            public static float easeOut(float t)
            {
                if (t < (1/2.75f))
                {
                    return 1;
                }
                if (t < (2/2.75f))
                {
                    t -= (1.5f/2.75f);
                    return 7.5625f*t*t + 0.75f;
                }
                if (t < (2.5f/2.75f))
                {
                    t -= (2.5f/2.75f);
                    return 7.5625f*t*t + 0.9375f;
                }
                t -= (2.625f/2.75f);
                return 7.5625f*t*t + 0.984375f;
            }


            public static float easeInOut(float t)
            {
                if (t <= 0.5f)
                    return easeIn(t*2)/2;
                return (easeOut((t - 0.5f)*2.0f)/2) + 0.5f;
            }
        }

        public static class Wiggle
        {
            private const float Factor = 1.0f;

            public static float easeIn(float t)
            {
                return Mathf.Sin(t*Mathf.PI*2.0f)*Factor;
            }

            public static float easeOut(float t)
            {
                return t;
            }


            public static float easeInOut(float t)
            {
                return t;
            }
        }
    }
}