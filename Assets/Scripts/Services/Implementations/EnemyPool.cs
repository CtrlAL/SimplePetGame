using ScriptableObjects;
using Services.Interfaces;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

namespace Services
{
    public class EnemyPool : IEnemyPool, IInitializable
    {
        [Inject] private DiContainer _diContainer;

        [Inject] private EnemyVariants _enemyLibrary;

        [Inject] private PoolingSettings _poolingSettings;

        private ConcurrentQueue<GameObject> _enemies = new();

        public void Initialize()
        {
            for (int i = 0; i < _poolingSettings.EnemyPoolSizeLimit; i++)
            {
                var prefab = _enemyLibrary.GetRandomEnemyPrefab();
                var enemy = _diContainer.InstantiatePrefab(prefab);
                enemy.SetActive(false);
                _enemies.Enqueue(enemy);
            }
        }

        public GameObject SpawnObject()
        {
            if (_enemies.TryDequeue(out var enemy) && enemy != null && enemy.scene.IsValid())
            {
                return enemy;
            }

            var prefab = _enemyLibrary.GetRandomEnemyPrefab();
            return _diContainer.InstantiatePrefab(prefab);
        }

        public void ReturnToPool(GameObject gameObject)
        {
            if (gameObject == null) return;

            var poolable = gameObject.GetComponent<IPoolableEnemy>();
            poolable?.ResetState();

            if (_enemies.Count < _poolingSettings.EnemyPoolSizeLimit)
            {
                gameObject.SetActive(false);
                _enemies.Enqueue(gameObject);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
