using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Presenters
{

    public class SpawnEnemyPresenter : IFixedTickable
    {
        [Inject] private IEnemyFactory _enemyFactory;
        [Inject] private LevelSettings _levelSettings;

        private float _timer = 0f;
        private float _currentCount = 0f;

        public void FixedTick()
        {
            _timer += Time.deltaTime;

            if (_timer >= _levelSettings.EnemySpawnRate && _currentCount < _levelSettings.EnemyMaximumCount)
            {
                _enemyFactory.CreateEnemy();
                _currentCount++;
                _timer = 0f;
            }
        }
    }
}