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

        private float _timer = 0f;

        private float _maxTimer = 4f;

        public void Awake()
        {
            _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint")
                .Select(x => x.transform)
                .ToArray();

            DestroyEnemyEventPublisher.Instance.DestroyEnemy += DestroyEnemy;
        }

        private void DestroyEnemy(object sender, GameObject args)
        {
            _enemyObjects.Remove(args);
            Destroy(args);
        }

        public void PublicUpdate()
        {
            _timer += Time.deltaTime;

            if (_timer >= _maxTimer && _enemyObjects.Count < _maxEnemyCount)
            {
                CreateEnemy();
                _timer = 0f;
            }
        }

        private void CreateEnemy()
        {
            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)];
            var enemy = Instantiate(_enemyPrefub, spawnPoint.position, spawnPoint.rotation);
            _enemyObjects.Add(enemy);
        }
    }
}
