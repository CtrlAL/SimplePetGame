using UnityEngine;

namespace Assets.Scripts
{
    public static class Helpers
    {
        //TODO: Сделать игока SinglTonom,и убрать эту функцию
        public static GameObject? FindPlayer()
        {
            return GameObject.FindWithTag("Player");
        }

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
