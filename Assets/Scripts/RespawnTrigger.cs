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
                other.gameObject.transform.position = _playerSpawnPoint.transform.position;
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

