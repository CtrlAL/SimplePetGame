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
        }

        public void CreateEnemy()
        {
            var spawnPoint = _spawnPoints[UnityRandom.Range(0, _spawnPoints.Length - 1)];
            var enemy = _pool.SpawnObject();
            enemy.transform.position = spawnPoint.position;
            enemy.transform.rotation = spawnPoint.rotation;

            var navMesh = enemy.GetComponent<NavMeshAgent>();
            navMesh.nextPosition = spawnPoint.transform.position;

            var rb = enemy.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            enemy.SetActive(true);
        }
    }
}