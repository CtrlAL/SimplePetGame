using Services.Interfaces;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityRandom = UnityEngine.Random;

namespace Services
{
    public class EnemyFactory : IEnemyFactory
    {
        private IEnemyPool _pool;

        private Transform[] _spawnPoints;

        private int _activeCount;

        public int TotalActive => _activeCount;

        public EnemyFactory(IEnemyPool enemyPool)
        {
            _pool = enemyPool;
            _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint")
                .Select(x => x.transform)
                .ToArray();
        }

        public void DestroyEnemy(GameObject args)
        {
            _pool.ReturnToPool(args);
            _activeCount = Mathf.Max(0, _activeCount - 1);
        }

        public GameObject CreateEnemy()
        {
            var spawnPoint = _spawnPoints[UnityRandom.Range(0, _spawnPoints.Length - 1)];
            var enemy = _pool.SpawnObject();
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;

            var rb = enemy.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            enemy.SetActive(true);

            var navMesh = enemy.GetComponent<NavMeshAgent>();
            navMesh.Warp(spawnPoint.position);
            navMesh.ResetPath();
            navMesh.velocity = Vector3.zero;

            var poolable = enemy.GetComponent<IPoolableEnemy>();
            poolable?.ResetState();

            _activeCount++;

            return enemy;
        }
    }
}