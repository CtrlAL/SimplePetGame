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
        private CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _respawnColiderViews.ForEach(x =>
            {
                x.OnCharacterFell
                .Subscribe(
                    co =>
                    {
                        if (co.IsEnemy())
                        {
                            _currentCount--;
                        }
                    }
                )
                .AddTo(_compositeDisposable);
            });
        }

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

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}