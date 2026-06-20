using UniRx;
using UnityEngine;

namespace Views
{
    public class RespawnColiderView : MonoBehaviour
    {
        public readonly Subject<Collider> OnCharacterFell = new();

        private void OnTriggerExit(Collider other)
        {
            OnCharacterFell?.OnNext(other);
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    OnCharacterFell?.OnNext(other);
        //}
    }
}