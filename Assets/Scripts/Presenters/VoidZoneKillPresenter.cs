using Extensions;
using Models;
using Services.Interfaces;
using System;
using System.Linq;
using UniRx;
using Zenject;

namespace Presenters
{
    public class VoidZoneKillPresenter : IInitializable, IDisposable
    {
        [Inject] private VoidZoneView _voidZoneView;
        [Inject] private GameOverModel _gameOverModel;
        [Inject] private IEnemyFactory _enemyFactory;
        [Inject] private IDeathEffectService _deathEffectView;

        private CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _voidZoneView.KillPerformed
                .Where(co => co.IsPlayer())
                .Subscribe(co =>
                {
                    KillPlayer(co);
                })
                .AddTo(_compositeDisposable); ;

            _voidZoneView.KillPerformed
                .Where(co => co.IsEnemy())
                .Subscribe(co =>
                {
                    KillEnemy(co);
                })
                .AddTo(_compositeDisposable);
        }

        private void KillEnemy(UnityEngine.Collider co)
        {
            _deathEffectView.ShowEffect(co.gameObject);
            _enemyFactory.DestroyEnemy(co.gameObject);
        }

        private void KillPlayer(UnityEngine.Collider co)
        {
            _gameOverModel.GameOver.OnNext(default);
            _deathEffectView.ShowEffect(co.gameObject);
            co.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}