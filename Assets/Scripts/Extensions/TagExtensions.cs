using Constants;
using UnityEngine;

namespace Extensions
{
    public static class TagExtensions
    {
        public static bool IsPlayer(this GameObject gameObject)
        {
            return gameObject.CompareTag(CharacterTags.Enemy) || gameObject.CompareTag(CharacterTags.BigEnemy);
        }

        public static bool IsPlayer(this Collider collider)
        {
            return collider.CompareTag(CharacterTags.Enemy) || collider.CompareTag(CharacterTags.BigEnemy);
        }

        public static bool IsEnemy(this GameObject gameObject)
        {
            return gameObject.CompareTag(CharacterTags.Enemy) || gameObject.CompareTag(CharacterTags.BigEnemy);
        }

        public static bool IsEnemy(this Collider collider)
        {
            return collider.CompareTag(CharacterTags.Enemy) || collider.CompareTag(CharacterTags.BigEnemy);
        }

        public static bool IsThrowable(this GameObject other)
        {
            return other.CompareTag(EnvironmentTags.Throwable);
        }

        public static bool IsThrowable(this Collider other)
        {
            return other.CompareTag(EnvironmentTags.Throwable);
        }
    }
}
