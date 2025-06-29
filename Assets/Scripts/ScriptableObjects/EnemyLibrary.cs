using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{

    [CreateAssetMenu(fileName = "NewEnemyLibrary", menuName = "Enemy/Enemy Library", order = 50)]
    public class EnemyLibrary : ScriptableObject
    {
        [SerializeField]
        private EnemyEntry[] _enemies;

        public EnemyEntry[] GetAllEnemies()
        {
            return _enemies;
        }

        public GameObject GetEnemyPrefab(int index)
        {
            if (index >= 0 && index < _enemies.Length)
            {
                return _enemies[index].Prefab;
            }
            Debug.LogWarning("Index out of range in EnemyLibrary");
            return null;
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

            public string Name => _name;
            public GameObject Prefab => _prefab;
        }
    }
}
