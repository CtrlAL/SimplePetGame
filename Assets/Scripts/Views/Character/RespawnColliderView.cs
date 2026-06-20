using System;
using UniRx;
using UnityEngine;

namespace Views
{
    public class RespawnColliderView : MonoBehaviour
    {
        private readonly Subject<Collider> _onCharacterFell = new();
        public IObservable<Collider> OnCharacterFell => _onCharacterFell;

        private void OnTriggerExit(Collider other)
        {
            _onCharacterFell?.OnNext(other);
        }

        private void OnDestroy()
        {
            _onCharacterFell?.Dispose();
        }
    }
}