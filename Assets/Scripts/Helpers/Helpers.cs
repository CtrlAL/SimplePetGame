using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts
{
    public static class Helpers
    {
        private static Vector3[] _ofsets = new Vector3[]
        {
            Vector3.zero,
            new Vector3(0.3f, 0, 0.3f),
            new Vector3(-0.3f, 0, 0.3f),
            new Vector3(0.3f, 0, -0.3f),
            new Vector3(-0.3f, 0, -0.3f),
        };

        public static bool IsPlayer(GameObject other)
        {
            return other.CompareTag(CharacterTags.Player);
        }

        public static bool IsEnemy(GameObject other)
        {
            return other.CompareTag(CharacterTags.Enemy);
        }

        public static bool IsThrowable(GameObject other)
        {
            return other.CompareTag(EnvironmentTags.Throwable);
        }

        public static bool IsGrounded(GameObject gameObject)
        {
            foreach (var offset in _ofsets)
            {
                Vector3 origin = gameObject.transform.position + offset;

                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 0.3f))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
