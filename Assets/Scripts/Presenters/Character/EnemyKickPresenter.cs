using Cysharp.Threading.Tasks;
using Extensions;
using FSM;
using ScriptableObjects;
using Services.Interfaces;
using Services.Sound;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class EnemyKickPresenter : IInitializable, IDisposable
    {
        [Inject] private CharacterFSM _fsm;
        [Inject] private EnemyKickZoneView _enemyKickView;
        [Inject] private AbstractStats _stats;
        [Inject] private IKicker _kicker;
        [Inject] private SoundManager _soundManager;

        private readonly Subject<Unit> _onKickPerformed = new();
        public IObservable<Unit> OnKickPerformed => _onKickPerformed;

        private readonly CompositeDisposable _compositeDisposable = new();
        private CancellationTokenSource _kickCts;

        public void Initialize()
        {
            _enemyKickView.PlayerEntered
                .Where(c => c.IsPlayer())
                .Subscribe(OnPlayerEntered)
                .AddTo(_compositeDisposable);

            _enemyKickView.PlayerExited
                .Where(c => c.IsPlayer())
                .Subscribe(_ => CancelKick())
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            CancelKick();
            _onKickPerformed?.Dispose();
            _compositeDisposable.Dispose();
        }

        private void OnPlayerEntered(Collider other)
        {
            if (!_fsm.IsIdleState()) return;

            CancelKick();
            _kickCts = new CancellationTokenSource();
            StartKickDelayed(other, _kickCts.Token).Forget();
        }

        private void CancelKick()
        {
            _kickCts?.Cancel();
            _kickCts?.Dispose();
            _kickCts = null;
        }

        private async UniTaskVoid StartKickDelayed(Collider other, CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_stats.DelayBeforeKick), cancellationToken: token);

                if (other == null || !_fsm.IsIdleState()) return;

                _kicker.Kick(other.gameObject, _stats.KickPower);
                _soundManager.PlaySound(0.5f, Enums.SoundType.EnemyKick);
                _onKickPerformed.OnNext(Unit.Default);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
