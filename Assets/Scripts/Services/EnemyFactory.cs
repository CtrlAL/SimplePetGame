using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using System.Linq;
using UnityEngine;
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
            var enemy = _pool.SpawnObject();
            var spawnPoint = _spawnPoints[UnityRandom.Range(0, _spawnPoints.Length - 1)];
            enemy.transform.position = spawnPoint.transform.position;
            enemy.transform.rotation = spawnPoint.rotation;
            enemy.SetActive(true);

            Debug.Break();
        }

        public void Dispose()
        {
            DestroyEnemyEventPublisher.Instance.DestroyEnemy -= DestroyEnemy;
        }
    }
}