using UnityEngine;

namespace Services.Helpers
{
    public static class GroundChecker
    {
        private static readonly int DefaultGroundLayer = ~Physics.IgnoreRaycastLayer;

        public static bool TryGetSurfaceNormal(
            Vector3 position,
            float maxDistance,
            LayerMask groundLayer,
            out Vector3 surfaceNormal,
            bool useSphereCast = false,
            float radius = 0.1f)
        {
            surfaceNormal = Vector3.up;

            bool hit = false;
            RaycastHit hitInfo;

            if (useSphereCast && radius > 0f)
            {
                hit = Physics.SphereCast(position, radius, Vector3.down, out hitInfo, maxDistance, groundLayer);
            }
            else
            {
                hit = Physics.Raycast(position, Vector3.down, out hitInfo, maxDistance, groundLayer);
            }

            if (hit)
            {
                surfaceNormal = hitInfo.normal;
                return true;
            }

            return false;
        }

        public static bool TryGetSurfaceNormal(
            Vector3 position,
            float maxDistance,
            out Vector3 surfaceNormal,
            bool useSphereCast = false,
            float radius = 0.1f)
        {
            return TryGetSurfaceNormal(
                position,
                maxDistance,
                DefaultGroundLayer,
                out surfaceNormal,
                useSphereCast,
                radius
            );
        }

        public static bool TryGetForwardNormal(Vector3 moveDirection, Transform characterTransform, float castDistance, float castHeightOffset, float castRadius, float maxSlopeAngle, out Vector3 normal)
        {
            normal = Vector3.up;
            Vector3 origin = characterTransform.position + Vector3.up * castHeightOffset;

            if (Physics.SphereCast(origin, castRadius, moveDirection, out RaycastHit hit, castDistance))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) <= maxSlopeAngle)
                {
                    normal = hit.normal;
                    return true;
                }
            }

            return false;
        }
    }
}