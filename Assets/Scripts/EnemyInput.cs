using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts
{
    public class EnemyInput : MonoBehaviour
    {
        [SerializeField] NavMeshAgent _agent;
        [SerializeField] Rigidbody _rigidbody;

        public void Awake()
        {
            _agent.updatePosition = false;
            _agent.updateRotation = true;
        }


        public void FixedUpdate()
        {
            if (PlayerInstanse.Instance != null && _agent != null && _rigidbody != null)
            {
                var target = PlayerInstanse.Instance.transform.position;
                _agent.SetDestination(target);
                var desiredVelocity = _agent.desiredVelocity;

                _rigidbody.MovePosition(_rigidbody.position + desiredVelocity * Time.fixedDeltaTime);
                _agent.nextPosition = transform.position;
            }
        }
    }
}
