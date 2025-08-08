using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "PoolingSettings", menuName = "PoolingSettings")]
    public class PoolingSettings : ScriptableObject
    {
        public int DeathEffectPoolSizeLimit = 10;

        public int StunEffectPoolSizeLimit = 10;

        public int EnemyPoolSizeLimit = 10;
    }
}
