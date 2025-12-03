using Models;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using UniRx;
using Views;
using Zenject;

namespace Presenters
{
    public class CharacterDeathPresenter : IInitializable, IDisposable
    {
        [Inject] private GameOverModel _gameOverModel;
        [Inject] private StatsModel _statsModel;
        [Inject] private IEnemyFactory _enemyFactory;
        [Inject] private List<RespawnColiderView> _respawnColiderViews;
        [Inject] private IDeathEffectService _deathEffectView;

        private CompositeDisposable _compositeDisposable;

        public void Initialize()
        {
            _compositeDisposable = new CompositeDisposable();

            _respawnColiderViews.ForEach(x =>
            {
                x.OnCharacterFell
                .Subscribe(
                    co =>
                    {
                        if (co.CompareTag("Player"))
                        {
                            _gameOverModel.GameOver.OnNext(default);
                            _deathEffectView.ShowEffect(co.gameObject);
                            co.gameObject.SetActive(false);
                        }

                        if (co.CompareTag("Enemy"))
                        {
                            _statsModel.KilledCubes.Value++;
                            _deathEffectView.ShowEffect(co.gameObject);
                            _enemyFactory.DestroyEnemy(co.gameObject);
                        }
                    }
                )
                .AddTo(_compositeDisposable);
            });
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
