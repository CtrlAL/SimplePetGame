using UniRx;
using UnityEngine;

namespace Views
{
    public class RespawnColiderView : MonoBehaviour
    {
        public readonly Subject<Collider> OnPlayerFell = new();

        public readonly Subject<Collider> OnEnemyFell = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerFell?.OnNext(other);
            }

            else if (other.CompareTag("Enemy"))
            {
                OnEnemyFell?.OnNext(other);
            }
        }
    }
}