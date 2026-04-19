using UnityEngine;

namespace Justcore
{
    public static class Extension
    {
        /// <summary>
        /// Rotates a 2D vector.
        /// </summary>
        public static Vector2 Rotate(this Vector2 v, float angleDeg)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);
            return new Vector2(cos * v.x - sin * v.y, sin * v.x + cos * v.y);
        }

        public static Vector3 RotateAroundAxis(Vector3 v, Vector3 axis, float angleDeg)
        {
            axis.Normalize();
            float angleRad = angleDeg * Mathf.Deg2Rad;
            return v * Mathf.Cos(angleRad)
                + Vector3.Cross(axis, v) * Mathf.Sin(angleRad)
                + axis * Vector3.Dot(axis, v) * (1f - Mathf.Cos(angleRad));
        }

        /// <summary>
        /// Converts a 2D vector into rotation.
        /// </summary>
        public static Quaternion ToRotation(this Vector2 vector)
        {
            Vector2 n = vector.normalized;
            return Quaternion.Euler(0, 0, Mathf.Atan2(n.y, n.x) * Mathf.Rad2Deg);
        }

        /// <summary>
        /// Adds Vector3(Random.Range(from, to), Random.Range(to, from), Random.Range(to, from)) to a vector.
        /// </summary>
        public static Vector3 Randomize(this Vector3 vector, float from, float to)
        {
            return vector + new Vector3(Random.Range(from, to), Random.Range(from, to), Random.Range(from, to));
        }

        public static T WeightedRandom<T>(T[] items, float[] weights)
        {
            if (items.Length != weights.Length)
                throw new System.ArgumentException("Items and weights must have the same length.");

            // Calculate total weight
            float totalWeight = 0f;
            for (int i = 0; i < weights.Length; i++)
                totalWeight += weights[i];

            // Get a random value in the range [0, totalWeight)
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            // Find which item corresponds to the random value
            for (int i = 0; i < items.Length; i++)
            {
                if (randomValue < weights[i])
                    return items[i];
                randomValue -= weights[i];
            }

            // Fallback (should never hit if weights > 0)
            return items[items.Length - 1];
        }

        /// <summary>
        /// Returns true is the positon defined by this vector is inside main camera's viewport.
        /// </summary>
        /// <param name="offsetInViewportUnits">Extends the check bounds. If the position is away from the bound by this amound, the method will return true anyway.</param>
        public static bool IsVisibleOnScreen(this Vector2 vector, Vector2 offsetInViewportUnits)
        {
            if (Camera.main == null)
                return false;

            Vector3 viewportPos = Camera.main.WorldToViewportPoint(vector);

            return viewportPos.z > 0 &&
                   viewportPos.x >= -offsetInViewportUnits.x && viewportPos.x <= 1 + offsetInViewportUnits.x &&
                   viewportPos.y >= -offsetInViewportUnits.y && viewportPos.y <= 1 + offsetInViewportUnits.y;
        }

        /// <inheritdoc cref="IsVisibleOnScreen(Vector2, Vector2)"/>
        public static bool IsVisibleOnScreen(this Vector2 vector)
        {
            return vector.IsVisibleOnScreen(Vector2.zero);
        }

        /// <summary>
        /// Transforms a value from one numerical interval into another, preserving proportional relationships.
        /// </summary>
        public static float Map(float value, float minOld, float maxOld, float minNew, float maxNew)
        {
            return minNew + (value - minOld) / (maxOld - minOld) * (maxNew - minNew);
        }

        /// <summary>
        /// Returns true if the GameObject belongs to the given layer mask.
        /// </summary>
        public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask)
        {
            return ((1 << obj.layer) & layerMask.value) != 0;
        }
    }
}