using UnityEngine;

namespace Justcore
{
    public static class Easing
    {
        static float OutQuad(float t)
        {
            return 1f - (1f - t) * (1f - t);
        }

        static float InQuad(float t)
        {
            return t * t;
        }

        static float InOutQuad(float t)
        {
            return t < 0.5f
                ? 2f * t * t
                : 1f - Mathf.Pow(-2f * t + 2f, 2f) / 2f;
        }
    }
}