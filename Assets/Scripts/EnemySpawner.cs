using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _enemyPrefub;

        private List<GameObject> _enemyObjects = new List<GameObject>();

        private Transform[] _spawnPoints;

        private int _maxEnemyCount = 10;

        private float timer = 0f;

        public void Awake()
        {
            _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint")
                .Select(x => x.transform)
                .ToArray();
        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= 10f && _enemyObjects.Count < _maxEnemyCount)
            {
                var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)];
                var enemy = Instantiate(_enemyPrefub, spawnPoint.position, spawnPoint.rotation);
                _enemyObjects.Add(enemy);

                timer = 0f;
            }
        }
    }
}
