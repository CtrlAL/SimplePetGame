using UnityEngine;

namespace ScriptableObjects
{
    public abstract class LevelSettings : ScriptableObject
    {
        public float EnemySpawnRate = 10;
        public float EnemyMaximumCount = 10;
    }
}
