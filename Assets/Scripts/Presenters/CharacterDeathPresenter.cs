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
        [Inject] private IEnemyFactory _enemyFactory;
        [Inject] private List<RespawnColiderView> _respawnColiderViews;
        [Inject] private IDeathEffectService _deathEffectView;

        private CompositeDisposable _compositeDisposable;
        public void Initialize()
        {
            _compositeDisposable = new CompositeDisposable();

            _respawnColiderViews.ForEach(x =>
            {
                x.OnEnemyFell
                .Subscribe(
                    co =>
                    {
                        _deathEffectView.ShowEffect(co.gameObject);
                        _enemyFactory.DestroyEnemy(co.gameObject);
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
