using FSM;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Services.EventPublishers
{
    public class EnemyAIMovementPublisher : MonoBehaviour
    {
        [Inject]
        private EnemyStatsSO _stats;

        [Inject]
        private CharacterFSM _fsm;

        [Inject]
        private Rigidbody _rigidbody;

        [SerializeField]
        private NavMeshAgent _agent;

        public void Awake()
        {
            _agent.updatePosition = false;
            _agent.updateRotation = true;
        }

        public void FixedUpdate()
        {
            if (PlayerInstanseHandler.Instance != null && _agent != null && _rigidbody != null)
            {
                AIUpdatePosition();
            }
        }

        private void AIUpdatePosition()
        {
            var target = PlayerInstanseHandler.Instance.transform.position;

            _agent.SetDestination(target);

            var desiredVelocity = _agent.desiredVelocity;

            _agent.nextPosition = transform.position;

            MoveEventPublisher.Instance.PublishMoveEvent(new Vector2(desiredVelocity.x, desiredVelocity.z), _fsm, _rigidbody, _stats.MoveSpeed);
        }
    }
}