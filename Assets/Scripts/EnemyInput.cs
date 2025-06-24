using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyInput : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (PlayerInstanse.Instance != null)
            {
                var targetPosition = PlayerInstanse.Instance.transform.position;
                Vector3 direction = (targetPosition - transform.position).normalized;
                var input = new Vector2(direction.x, direction.z);
                MoveEventPublisher.Instance.PublishMoveEvent(input, gameObject);
            }
        }
    }
}
