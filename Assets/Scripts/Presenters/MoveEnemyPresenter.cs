using Extensions;
using FSM;
using ScriptableObjects;
using Services;
using UnityEngine.AI;
using Zenject;

namespace Presenters
{
    public class MoveEnemyPresenter : IFixedTickable, IInitializable
    {
        [Inject] private CharacterFSM _fsm;

        [Inject] private NavMeshAgent _navMeshAgent;

        [Inject] private EnemyStats _stats;

        public void Initialize()
        {
            _navMeshAgent.updatePosition = true;
            _navMeshAgent.updateRotation = true;
            _navMeshAgent.speed = _stats.MoveSpeed;
            _navMeshAgent.angularSpeed = _stats.RotationSpeed;
        }

        public void FixedTick()
        {
            if (PlayerInstanseHandler.Instance == null)
            {
                return;
            }

            var newPosition = PlayerInstanseHandler.Instance.transform.position;

            if (_fsm.IsIdleState() && _navMeshAgent.destination != newPosition)
            {
                var target = PlayerInstanseHandler.Instance.transform.position;
                _navMeshAgent.destination = target;
            }
        }
    }
}