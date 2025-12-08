using UnityEngine;

namespace ScriptableObjects
{

    [CreateAssetMenu(fileName = "EnemyVariants", menuName = "Enemy/EnemyVariants", order = 50)]
    public class EnemyVariants : ScriptableObject
    {
        [SerializeField]
        private EnemyEntry[] _enemies;

        public EnemyEntry[] GetAllEnemies()
        {
            return _enemies;
        }

        public GameObject GetRandomEnemyPrefab()
        {
            if (_enemies == null || _enemies.Length == 0)
            {
                Debug.LogError("No enemy entries defined!");
                return null;
            }

            float[] weights = new float[_enemies.Length];
            float totalWeight = 0f;

            for (int i = 0; i < _enemies.Length; i++)
            {
                float rate = Mathf.Max(_enemies[i].SpawnRate, 1f);
                weights[i] = 1f / rate;
                totalWeight += weights[i];
            }

            if (totalWeight <= 0f)
            {
                Debug.LogWarning("Total weight is zero. Returning first enemy.");
                return _enemies[0].Prefab;
            }

            float random = Random.Range(0f, totalWeight);
            float cumulative = 0f;

            for (int i = 0; i < _enemies.Length; i++)
            {
                cumulative += weights[i];
                if (random <= cumulative)
                {
                    return _enemies[i].Prefab;
                }
            }

            return _enemies[^1].Prefab;
        }

        public int GetLength()
        {
            return _enemies.Length;
        }

        [System.Serializable]
        public class EnemyEntry
        {
            [SerializeField] private string _name;
            [SerializeField] private GameObject _prefab;
            [SerializeField, Min(1f)] private float _spawnRate;

            public string Name => _name;
            public GameObject Prefab => _prefab;
            public float SpawnRate => _spawnRate;
        }
    }
}
