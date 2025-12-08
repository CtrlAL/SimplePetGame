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
            for (int i = 0; i < _poolingSettings.EnemyPoolSizeLimit + _poolingSettings.BigEnemyPoolSizeLimit; i++)
            {
                _enemies.Enqueue(_enemyLibrary.GetRandomEnemyPrefab());
            }
        }

        public GameObject SpawnObject()
        {
            GameObject enemy = null;

            while (_enemies.Count > 0) 
            {
                if (_enemies.TryDequeue(out enemy) && enemy == null)
                {
                    continue;
                }
            }

            if (enemy != null && enemy.scene.IsValid())
            {
                return enemy;
            }
            else
            {
                if (enemy == null)
                {
                    _enemies.Clear();
                }

                var prefub = _enemyLibrary.GetRandomEnemyPrefab();
                enemy = _diContainer.InstantiatePrefab(prefub);

                return enemy;
            }
        }

        public void ReturnToPool(GameObject gameObject)
        {
            if (_poolingSettings.EnemyPoolSizeLimit >= _enemies.Count)
            {
                gameObject.gameObject.SetActive(false);
                _enemies.Enqueue(gameObject);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
