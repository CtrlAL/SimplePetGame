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

        public static bool TryGetForwardNormal(Vector3 moveDirection, Transform characterTransform, float castDistance, float castRadius, float maxSlopeAngle, out Vector3 normal)
        {
            normal = Vector3.up;
            var baseOffset = 1f;

            if (Physics.SphereCast(characterTransform.position, castRadius, Vector3.down, out RaycastHit groudhit, castDistance, DefaultGroundLayer) && 
                Physics.SphereCast(groudhit.point + Vector3.up * baseOffset, castRadius, moveDirection, out RaycastHit hit, castDistance, DefaultGroundLayer))
            {
                Debug.DrawRay(characterTransform.position, Vector3.down * 10, Color.red);
                Debug.DrawRay(groudhit.point, moveDirection * 10, Color.blue);

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