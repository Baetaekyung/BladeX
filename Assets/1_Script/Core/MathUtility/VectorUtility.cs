using UnityEngine;

namespace Swift_Blade
{
    public static class VectorUtility
    {
        /// <summary>
        /// Returns true if the distance between two points is in a range.
        /// </summary>
        public static bool IsInRange(this Vector3 source, Vector3 target, float range)
        {
            bool result = Vector3.SqrMagnitude(source - target) < range * range;
            return result;
        }
        /// <summary>
        /// Returns true if the distance between two points is in a range.
        /// </summary>
        /// <param name="squaredRange">range that is squared</param>
        public static bool IsInRangeSquared(this Vector3 source, Vector3 target, float squaredRange)
        {
            bool result = Vector3.SqrMagnitude(source - target) < squaredRange;
            return result;
        }
    }
}
