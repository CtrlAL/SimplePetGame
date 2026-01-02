using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "VoidZoneSpawnSettings", menuName = "VoidZoneSpawnSettings")]
    public class VoidZoneSpawnSettings : ScriptableObject
    {
        public GameObject Prefub;
        public int SpawnCount = 10;
        public float SpawnInterval = 2f;
        public float Lifetime = 10f;
        public float HeightOffset = 0.1f;
    }
}
