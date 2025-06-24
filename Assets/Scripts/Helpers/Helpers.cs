using UnityEngine;

namespace Assets.Scripts
{
    public static class Helpers
    {
        public static bool IsPlayer(GameObject other)
        {
            return other.CompareTag("Player");
        }

        public static bool  IsEnemy(GameObject other)
        {
            return other.CompareTag("Enemy");
        }
    }
}
