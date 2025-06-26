using UnityEngine;
using Assets.Scripts.Constants;

namespace Assets.Scripts
{
    public static class Helpers
    {
        public static bool IsPlayer(GameObject other)
        {
            return other.CompareTag(CommomnConstants.CharacterTags.Player);
        }

        public static bool IsEnemy(GameObject other)
        {
            return other.CompareTag(CommomnConstants.CharacterTags.Enemy);
        }

        public static bool IsThrowable(GameObject other)
        {
            return other.CompareTag(CommomnConstants.EnvironmentTags.Throwable);
        }
    }
}
