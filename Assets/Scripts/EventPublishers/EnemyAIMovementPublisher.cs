using Assets.Scripts.FSM;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CharacterFSM))]
    public class EnemyAIMovementPublisher : MonoBehaviour
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

            var fsm = gameObject.GetComponent<CharacterFSM>();
            var rb = gameObject.GetComponent<Rigidbody>();

            MoveEventPublisher.Instance.PublishMoveEvent(new Vector2(desiredVelocity.x, desiredVelocity.z), fsm, rb, _stats.MoveSpeed);
        }
    }
}