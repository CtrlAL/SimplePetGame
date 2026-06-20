using UniRx;
using UnityEngine;

namespace Views
{
    public class ThrowableInteractionView : MonoBehaviour
    {
        public GameObject ThrowablesSlot;

        private readonly Subject<GameObject> _onObjectEnteredRange = new();
        private readonly Subject<GameObject> _onObjectStayedInRange = new();
        private readonly Subject<GameObject> _onObjectExitedRange = new();

        public IObservable<GameObject> OnObjectEnteredRange => _onObjectEnteredRange;
        public IObservable<GameObject> OnObjectStayedInRange => _onObjectStayedInRange;
        public IObservable<GameObject> OnObjectExitedRange => _onObjectExitedRange;

        private void OnTriggerEnter(Collider other) =>
            _onObjectEnteredRange?.OnNext(other.gameObject);

        private void OnTriggerStay(Collider other) =>
            _onObjectStayedInRange?.OnNext(other.gameObject);

        private void OnTriggerExit(Collider other) =>
            _onObjectExitedRange?.OnNext(other.gameObject);

        private void OnDestroy()
        {
            _onObjectEnteredRange?.Dispose();
            _onObjectStayedInRange?.Dispose();
            _onObjectExitedRange?.Dispose();
        }
    }
}