using UniRx;
using UnityEngine;

namespace Views
{
    public class EnemyKickZoneView : MonoBehaviour
    {
        public Subject<Collider> PlayerEntered = new();
        public Subject<Unit> PlayerExited = new();

        private void OnTriggerEnter(Collider other)
        {
            PlayerEntered.OnNext(other);
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerExited.OnNext(Unit.Default);
        }

        private void OnDestroy()
        {
            PlayerEntered?.Dispose();
            PlayerExited?.Dispose();
        }
    }
}
