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
        [Inject] private EnemyStats _stats;
        [Inject] private IKicker _kicker;
        [Inject] private SoundManager _soundManager;

        public readonly Subject<Unit> OnKickPerformed = new();

        private readonly CompositeDisposable _compositeDisposable = new();
        private CancellationTokenSource _kickCts;

        public void Initialize()
        {
            _enemyKickView.PlayerEntered
                .Subscribe(OnPlayerEntered)
                .AddTo(_compositeDisposable);

            _enemyKickView.PlayerExited
                .Subscribe(_ => CancelKick())
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            CancelKick();
            OnKickPerformed?.Dispose();
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

                if (other != null)
                {
                    _kicker.Kick(other.gameObject, _stats.KickPower);
                    _soundManager.PlaySound(0.5f, Enums.SoundType.EnemyKick);
                    OnKickPerformed.OnNext(Unit.Default);
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
