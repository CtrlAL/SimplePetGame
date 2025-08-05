using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;


namespace Services
{
    public class EnemyFactory : IEnemyFactory
    {
        private DiContainer _container;

        private EnemyLibrary _enemyPrefubLibrary;

        private List<GameObject> _enemyObjects;

        private Transform[] _spawnPoints;

        private int _maxEnemyCount = 10;

        private float _timer = 0f;

        private float _maxTimer = 4f;

        public EnemyFactory(DiContainer container, EnemyLibrary enemyLibrary)
        {
            _container = container;

            _enemyPrefubLibrary = enemyLibrary;

            _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint")
                .Select(x => x.transform)
                .ToArray();

            DestroyEnemyEventPublisher.Instance.DestroyEnemy += DestroyEnemy;

            _enemyObjects = new();
        }

        public void DestroyEnemy(object sender, GameObject args)
        {
            _enemyObjects.Remove(args);
            GameObject.Destroy(args);
        }

        public void Tick()
        {
            _timer += Time.deltaTime;

            if (_timer >= _maxTimer && _enemyObjects.Count < _maxEnemyCount)
            {
                CreateEnemy();
                _timer = 0f;
            }
        }

        public void CreateEnemy()
        {
            var index = Random.Range(0, _enemyPrefubLibrary.GetLength());
            var prefub = _enemyPrefubLibrary.GetEnemyPrefab(index);

            var spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length - 1)];
            var enemy =  _container.InstantiatePrefab(prefub, spawnPoint.position, spawnPoint.rotation, spawnPoint);
            _enemyObjects.Add(enemy);
        }

        public void Dispose()
        {
            DestroyEnemyEventPublisher.Instance.DestroyEnemy -= DestroyEnemy;
        }
    }
}