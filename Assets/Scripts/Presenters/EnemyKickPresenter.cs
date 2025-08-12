using FSM;
using ScriptableObjects;
using Services.Interfaces;
using Services.EventPublishers;
using UnityEngine;
using Zenject;
using Views;
using UniRx;
using System;
using Extensions;

namespace Presenters
{
    public class EnemyKickPresenter : IInitializable, IDisposable
    {
        [Inject] private CharacterFSM _fsm;

        [Inject] private EnemyKickView _enemyKickView;

        [Inject] private EnemyStatsSO _stats;

        [Inject] private IKiker _kicker;

        private CompositeDisposable _disposables;

        public void Initialize()
        {
            _enemyKickView.KickPerformed
                .Subscribe(Kick)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void Kick(Collider other)
        {
            if (other != null && _fsm.IsIdleState())
            {
                var kickEventArgs = new KickEventArgs(_fsm.GameObject, other.gameObject, _stats.KickPower);
                _kicker.Kick(kickEventArgs);
            }
        }
    }
}
