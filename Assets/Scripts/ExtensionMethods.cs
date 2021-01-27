using UnityEngine;

namespace Assets.Scripts
{
    static class ExtensionMethods
    {
        public static Vector3 Add(this Vector3 originalVector, Vector3 newVector)
        {
            return originalVector.Add(newVector.x, newVector.y, newVector.z);
        }

        public static Vector3 Add(this Vector3 originalVector, float x, float y, float z)
        {
            return new Vector3(originalVector.x + x, originalVector.y + y, originalVector.z + z);
        }

        public static bool ContainsBounds(this Bounds bounds, Bounds target)
        {
            return bounds.Contains(target.min) && bounds.Contains(target.max);
        }

        public static Vector3 Set2D(this Vector3 originalVector, Vector3 newVector)
        {
            return new Vector3(newVector.x, newVector.y, originalVector.z);
        }
    }
}
