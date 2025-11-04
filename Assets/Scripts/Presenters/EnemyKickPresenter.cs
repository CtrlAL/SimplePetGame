using FSM;
using ScriptableObjects;
using Services.Interfaces;
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

        [Inject] private EnemyKickZoneView _enemyKickView;

        [Inject] private EnemyStats _stats;

        [Inject] private IKiker _kicker;

        private CompositeDisposable _disposables = new();

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
                _kicker.Kick(other.gameObject, _stats.KickPower);
            }
        }
    }
}
