using Cysharp.Threading.Tasks;
using ScriptableObjects;
using Services;
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

        public void OnTriggerEnter(Collider other)
        {
            var token = _cancellationTokenSource.Token;

            if (other.gameObject == PlayerInstanseHandler.Instance)
            {
                UniTask.RunOnThreadPool(async () =>
                {
                    await UniTask.WaitForSeconds(_enemyStatsSO.DelayBeforeKick);
                    KickPerformed.OnNext(other);
                }, true, token);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            _cancellationTokenSource?.Cancel();
        }
    }
}