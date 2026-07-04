using UnityEngine;
using Constants;

namespace Helpers
{
    public static class GameHelpers
    {
        public static bool IsThrowable(GameObject other)
        {
            return other.CompareTag(EnvironmentTags.Throwable);
        }

        public static bool IsGrounded(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent<Collider>(out var collider))
                return false;

            float halfHeight = collider.bounds.extents.y;
            Vector3 origin = gameObject.transform.position + Vector3.up * 0.05f;

            return Physics.Raycast(origin, Vector3.down, halfHeight + 0.15f);
        }
    }
}
