using UniRx;
using UnityEngine;

namespace Views.Scene
{
    public class ThrowableInteractionView : MonoBehaviour
    {
        public GameObject ThrowablesSlot;

        public Subject<GameObject> OnObjectEnteredRange = new();
        public Subject<GameObject> OnObjectStayedInRange = new();
        public Subject<GameObject> OnObjectExitedRange = new();

        private void OnTriggerEnter(Collider other) =>
            OnObjectEnteredRange?.OnNext(other.gameObject);

        private void OnTriggerStay(Collider other) =>
            OnObjectStayedInRange?.OnNext(other.gameObject);

        private void OnTriggerExit(Collider other) =>
            OnObjectExitedRange?.OnNext(other.gameObject);
    }
}

