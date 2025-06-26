using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts
{
    public static class Helpers
    {
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
    }
}
