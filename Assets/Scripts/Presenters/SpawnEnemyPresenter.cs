using Extensions;
using ScriptableObjects;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{

    public class SpawnEnemyPresenter : IFixedTickable, IInitializable, IDisposable
    {
        [Inject] private IEnemyFactory _enemyFactory;
        [Inject] private LevelSettings _levelSettings;
        [Inject] private List<RespawnColiderView> _respawnColiderViews;

        private float _timer = 0f;
        private float _currentCount = 0f;
        private float _bigCurrentCount = 0f;

        private CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _respawnColiderViews.ForEach(x =>
            {
                x.OnCharacterFell
                .Where(co => co.IsBigEnemy())
                .Subscribe(co => _bigCurrentCount--)
                .AddTo(_compositeDisposable);

                x.OnCharacterFell
                .Where(co => co.IsDefaultEnemy())
                .Subscribe(co => _currentCount--)
                .AddTo(_compositeDisposable);
            });
        }

        public void FixedTick()
        {
            _timer += Time.deltaTime;

            if (_timer >= _levelSettings.EnemySpawnRate && _currentCount + _bigCurrentCount < _levelSettings.EnemyMaximumCount + _levelSettings.BigEnemyMaximumCount)
            {
                var enemy = _enemyFactory.CreateEnemy();

                if (enemy.gameObject.IsBigEnemy())
                {
                    _bigCurrentCount++;
                }

                if (enemy.gameObject.IsDefaultEnemy())
                {
                    _currentCount++;
                }

                _timer = 0f;
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}