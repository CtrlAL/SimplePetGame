using UnityEngine;

namespace ScriptableObjects.Extensions
{
    public static class EnemyEntryExtensions
    {
        public static GameObject GetRandomEnemyPrefab(this EnemyVariants.EnemyEntry[] enemyEntries)
        {
            if (enemyEntries == null || enemyEntries.Length == 0)
            {
                Debug.LogError("No enemy entries defined!");
                return null;
            }

            float[] weights = new float[enemyEntries.Length];
            float totalWeight = 0f;

            for (int i = 0; i < enemyEntries.Length; i++)
            {
                float rate = Mathf.Max(enemyEntries[i].SpawnRate, 1f);
                weights[i] = 1f / rate;
                totalWeight += weights[i];
            }

            if (totalWeight <= 0f)
            {
                Debug.LogWarning("Total weight is zero. Returning first enemy.");
                return enemyEntries[0].Prefab;
            }

            float random = UnityEngine.Random.Range(0f, totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < enemyEntries.Length; i++)
            {
                cumulative += weights[i];
                if (random <= cumulative)
                {
                    return enemyEntries[i].Prefab;
                }
            }

            return enemyEntries[^1].Prefab;
        }
    }
}