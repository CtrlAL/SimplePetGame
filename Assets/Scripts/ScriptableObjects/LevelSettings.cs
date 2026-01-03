using UnityEngine;

namespace ScriptableObjects
{
    public abstract class LevelSettings : ScriptableObject
    {
        public float EnemySpawnRate = 10;
        public float EnemyMaximumCount = 10;
        public float BigEnemyMaximumCount = 3;

        [Tooltip("Level duration in minutes (automatically converted to seconds)")]
        public float LevelDuration = 15;

        public bool EnableSpawn = true;
        public bool VoidZoneSpawnEnable = true;
    }
}