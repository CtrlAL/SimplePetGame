using FSM;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Zenject;
using Views;
using UniRx;
using System;
using Extensions;
using Services.Sound;

namespace Presenters
{
    public class EnemyKickPresenter : IInitializable, IDisposable
    {
        [Inject] private CharacterFSM _fsm;

        [Inject] private EnemyKickZoneView _enemyKickView;

        [Inject] private EnemyStats _stats;

        [Inject] private IKiker _kicker;

        [Inject] SoundManager _soundManager;

        private CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _enemyKickView.KickPerformed
                .Subscribe(Kick)
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        private void Kick(Collider other)
        {
            if (other != null && _fsm.IsIdleState())
            {
                _kicker.Kick(other.gameObject, _stats.KickPower);
                _soundManager.PlaySound(0.5f, Enums.SoundType.EnemyKick);
            }
        }
    }
}
