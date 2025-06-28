using UnityEngine;

namespace Assets.Scripts
{
    public class RespawnTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject _playerSpawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && CheckTag())
            {
                var rb = other.attachedRigidbody;
                rb.MovePosition(_playerSpawnPoint.transform.position);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            else if (other.CompareTag("Enemy"))
            {
                DestroyEnemyEventPublisher.Instance.PublishEvent(other.gameObject);
            }
        }

        private bool CheckTag()
        {
            if (_playerSpawnPoint.tag == "SpawnPoint")
            {
                return true;
            }
            return false;
        }
    }
}