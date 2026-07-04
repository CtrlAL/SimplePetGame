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

        public void FixedTick()
        {
            if (!_levelSettings.EnableSpawn)
            {
                return;
            }

            _timer += Time.deltaTime;

            int maxTotal = (int)(_levelSettings.EnemyMaximumCount + _levelSettings.BigEnemyMaximumCount);

            if (_timer >= _levelSettings.EnemySpawnRate && _enemyFactory.TotalActive < maxTotal)
            {
                _enemyFactory.CreateEnemy();
                _timer = 0f;
            }
        }
    }
}