using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class EnemyInput : MonoBehaviour
    {
        [SerializeField]
        private EnemyStatsSO _stats;

        [SerializeField] 
        private NavMeshAgent _agent;

        [SerializeField] 
        private Rigidbody _rigidbody;

        public void Awake()
        {
            _agent.updatePosition = false;
            _agent.updateRotation = true;
        }

        public void FixedUpdate()
        {
            if (PlayerInstanse.Instance != null && _agent != null && _rigidbody != null)
            {
                AIUpdatePosition();
            }
        }

        private void AIUpdatePosition()
        {
            var target = PlayerInstanse.Instance.transform.position;

            _agent.SetDestination(target);

            var desiredVelocity = _agent.desiredVelocity;

            _agent.nextPosition = transform.position;

            MoveEventPublisher.Instance.PublishMoveEvent(new Vector2(desiredVelocity.x, desiredVelocity.z), gameObject, _stats.MoveSpeed);
        }
    }
}
