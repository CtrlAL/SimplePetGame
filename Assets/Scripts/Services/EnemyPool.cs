using ScriptableObjects;
using Services.Interfaces;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

namespace Services
{
    public class EnemyPool : IEnemyPool
    {
        [Inject] private readonly DiContainer _diContainer;

        [Inject] private EnemyLibrary _enemyLibrary;

        [Inject] private PoolingSettings _poolingSettings;

        private ConcurrentQueue<GameObject> _enemies = new();

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

                var index = Random.Range(0, _enemyLibrary.GetLength());
                var prefub = _enemyLibrary.GetEnemyPrefab(index);

                return GameObject.Instantiate(prefub);
                //return _diContainer.InstantiatePrefab(prefub);
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
