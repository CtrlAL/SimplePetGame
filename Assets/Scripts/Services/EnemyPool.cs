using ScriptableObjects;
using Services.Interfaces;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Services
{
    public class EnemyPool : IEnemyPool
    {
        [Inject] private readonly DiContainer _diContainer;

        [Inject] private EnemyLibrary _enemyLibrary;

        [Inject] private PoolingSettings _poolingSettings;

        private ConcurrentBag<GameObject> _enemies = new();

        public GameObject SpawnObject()
        {
            if (_enemies.TryPeek(out var enemy))
            {
                return enemy;
            }
            else
            {
                var index = Random.Range(0, _enemyLibrary.GetLength());
                var prefub = _enemyLibrary.GetEnemyPrefab(index);

                return _diContainer.InstantiatePrefab(prefub);
            }
        }

        public void ReturnToPool(GameObject gameObject)
        {
            if (_poolingSettings.EnemyPoolSizeLimit >= _enemies.Count)
            {
                gameObject.gameObject.SetActive(false);
                _enemies.Add(gameObject);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
