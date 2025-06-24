using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyInput : MonoBehaviour
    {
        [SerializeField]
        private MoveEventPublisher _moveEventPublisher;

        private GameObject _playerObject;

        public void FixedUpdate()
        {
            if (_playerObject == null)
            {
                _playerObject = Helpers.FindPlayer();
            }

            if (_playerObject != null)
            {
                var targetPosition = _playerObject.transform.position;
                Vector3 direction = (targetPosition - transform.position).normalized;
                var input = new Vector2(direction.x, direction.z);

                _moveEventPublisher.PublishMoveEvent(input, gameObject);
            }
        }
    }
}
