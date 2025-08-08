using Services.EventPublishers;
using UniRx;
using UnityEngine;

namespace Views
{
    public class RespawnColiderView : MonoBehaviour
    {
        [SerializeField]
        private GameObject PlayerSpawnPoint;

        public Subject<Collider> OnPlayerFell = new();

        public Subject<Collider> OnEnemyFell = new();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                OnPlayerFell.OnNext(other);
            }

            else if (other.CompareTag("Enemy"))
            {
                OnEnemyFell.OnNext(other);
                DestroyEnemyEventPublisher.Instance.PublishEvent(other.gameObject);
            }
        }
    }
}