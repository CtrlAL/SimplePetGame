using FMODUnity;
using System.Linq;
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

        public static bool TryGetForwardNormal(Vector3 moveDirection, Transform characterTransform, float castDistance, float maxSlopeAngle, out Vector3 normal)
        {
            normal = Vector3.up;
            var baseOffset = 0.1f;

            if (Physics.Raycast(characterTransform.position, Vector3.down, out RaycastHit groudhit, castDistance, DefaultGroundLayer) && 
                Physics.Raycast(groudhit.point + Vector3.up * baseOffset, moveDirection, out RaycastHit hit, castDistance, DefaultGroundLayer))
            {
                Debug.DrawRay(groudhit.point + Vector3.up * baseOffset, moveDirection, Color.blue, 10f, false);

                var angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle <= maxSlopeAngle)
                {
                    normal = hit.normal;
                    return true;
                }
            }

            return false;
        }

        public static bool IsCompletelyOffPlatform(BoxCollider colider, Transform transform, LayerMask platformLayer)
        {
            if (colider == null) return true;

            Vector3 center = colider.center;
            Vector3 size = colider.size;

            Vector3[] localCorners = {
                center + new Vector3(-size.x, -size.y, -size.z) * 0.5f,
                center + new Vector3( size.x, -size.y, -size.z) * 0.5f,
                center + new Vector3(-size.x, -size.y,  size.z) * 0.5f,
                center + new Vector3( size.x, -size.y,  size.z) * 0.5f
            };

            Vector3[] worldCorners = localCorners.Select(local => transform.TransformPoint(local)).ToArray();

            var hits = worldCorners
                .Select(corner =>
                {
                    var result = Physics.Raycast(corner, Vector3.down, out RaycastHit hit, 0.1f, platformLayer);
                    Debug.DrawRay(corner, Vector3.down, Color.yellow, 10f);
                    return new { Result = result, Hit = hit };
                }).ToList();

            var transforms = hits.Where(x => x.Result == true).Select(y => y.Hit.transform).ToList();
            var differentTransforms = transforms.Distinct().ToList();

            return differentTransforms.Count != 1;
        }


        public static bool IsCompletelyOffPlatform(BoxCollider colider, Transform transform)
        {
            return IsCompletelyOffPlatform(colider, transform, DefaultGroundLayer);
        }
    }
}