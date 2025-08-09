using FSM;
using Models;
using ScriptableObjects;
using Services;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Services.EventPublishers;

namespace Assets.Scripts.Presenters
{
    
    public class MoveEnemyPresenter : IFixedTickable, IInitializable
    {
        [Inject] 
        private MoveCharacterModel _moveCharacterModel;

        [Inject] 
        private IMover _mover;

        [Inject] 
        private NavMeshAgent _navMeshAgent;

        [Inject] 
        private EnemyStatsSO _stats;

        [Inject] 
        private CharacterFSM _fsm;

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

            _mover.Move(this, new MoveEventArgs(input, _fsm, _moveCharacterModel.Rigidbody, _stats.MoveSpeed, _stats.RotationSpeed));
        }
    }
}
