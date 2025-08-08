using System;
using UniRx;
using UnityEngine;

namespace Views
{
    public class RespawnColiderView : MonoBehaviour
    {
        public readonly Subject<Collider> OnPlayerFell = new();

        public readonly Subject<Collider> OnEnemyFell = new();

        public event Action<Collider> AHAHAHAHAWORK;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                AHAHAHAHAWORK?.Invoke(other);
                OnPlayerFell?.OnNext(other);
            }

            else if (other.CompareTag("Enemy"))
            {
                OnEnemyFell?.OnNext(other);
            }
        }
    }
}