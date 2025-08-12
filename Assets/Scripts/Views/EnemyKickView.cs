using Cysharp.Threading.Tasks;
using ScriptableObjects;
using Services;
using System;
using System.Threading;
using UniRx;
using UnityEngine;
using Zenject;

namespace Views
{
    public class EnemyKickView : MonoBehaviour
    {
        [Inject] private EnemyStatsSO _enemyStatsSO;

        public Subject<Collider> KickPerformed = new();

        private CancellationTokenSource _cancellationTokenSource = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject != PlayerInstanseHandler.Instance) return;

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();

            StartKickDelay(other, _cancellationTokenSource.Token).Forget();
        }

        private async UniTaskVoid StartKickDelay(Collider other, CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_enemyStatsSO.DelayBeforeKick), cancellationToken: token);

                KickPerformed.OnNext(other);
            }
            catch (OperationCanceledException)
            {

            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == PlayerInstanseHandler.Instance)
            {
                _cancellationTokenSource?.Cancel();
            }
        }

        private void OnDestroy()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            KickPerformed?.Dispose();
        }
    }
}