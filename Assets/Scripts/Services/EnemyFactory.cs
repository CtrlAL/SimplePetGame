using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using UnityRandom = UnityEngine.Random;

namespace Services
{
    public class EnemyFactory : IEnemyFactory, IFixedTickable, IDisposable
    {
        private IEnemyPool _pool;

        private LevelSettings _levelSettings;

        private Transform[] _spawnPoints;

        private float _timer = 0f;

        private int _currentCount = 0;

        public EnemyFactory(IEnemyPool enemyPool, LevelSettings levelSettings)
        {
            _pool = enemyPool;

            _levelSettings = levelSettings;

            _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint")
                .Select(x => x.transform)
                .ToArray();

            DestroyEnemyEventPublisher.Instance.DestroyEnemy += DestroyEnemy;
        }

        public void DestroyEnemy(GameObject args)
        {
            _pool.ReturnToPool(args);
            _currentCount--;
        }

        public void FixedTick()
        {
            _timer += Time.deltaTime;

            if (_timer >= _levelSettings.EnemySpawnRate && _currentCount < _levelSettings.EnemyMaximumCount)
            {
                CreateEnemy();
                _currentCount++;
                _timer = 0f;
            }
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

        public void Dispose()
        {
            DestroyEnemyEventPublisher.Instance.DestroyEnemy -= DestroyEnemy;
        }
    }
}