using Models;
using ScriptableObjects;
using Services;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Presenters
{
    
    public class MoveEnemyPresenter : IFixedTickable, IInitializable
    {
        [Inject] private MoveCharacterModel _moveCharacterModel;

        [Inject] private IMover _mover;

        [Inject] private NavMeshAgent _navMeshAgent;

        [Inject] private EnemyStats _stats;

        public void Initialize()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
        }

        public void FixedTick()
        {
            if (PlayerInstanseHandler.Instance == null)
            {
                return;
            }

            var target = PlayerInstanseHandler.Instance.transform.position;

            _navMeshAgent.SetDestination(target);

            var desiredVelocity = _navMeshAgent.desiredVelocity;

            _navMeshAgent.nextPosition = _moveCharacterModel.Transform.position;

            var input = new Vector2(desiredVelocity.x, desiredVelocity.z);

            _mover.Move(input, _stats.MoveSpeed, _stats.RotationSpeed);
        }
    }
}
