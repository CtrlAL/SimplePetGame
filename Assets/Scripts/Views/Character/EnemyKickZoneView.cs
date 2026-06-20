using UniRx;
using UnityEngine;

namespace Views
{
    public class EnemyKickZoneView : MonoBehaviour
    {
        private readonly Subject<Collider> _playerEntered = new();
        private readonly Subject<Collider> _playerExited = new();

        public IObservable<Collider> PlayerEntered => _playerEntered;
        public IObservable<Collider> PlayerExited => _playerExited;

        private void OnTriggerEnter(Collider other)
        {
            _playerEntered.OnNext(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _playerExited.OnNext(other);
        }

        private void OnDestroy()
        {
            _playerEntered?.Dispose();
            _playerExited?.Dispose();
        }
    }
}
