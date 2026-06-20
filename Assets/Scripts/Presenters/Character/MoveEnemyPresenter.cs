using Models;
using ScriptableObjects;
using Services.Helpers;
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
        [Inject] private AbstractStats _stats;
        [Inject] private IPlayerProvider _playerProvider;

        public void Initialize()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
        }

        public void FixedTick()
        {
            if (!_navMeshAgent.enabled) return;

            var target = _playerProvider.Instance.transform.position;

            _navMeshAgent.SetDestination(target);

            var desiredVelocity = _navMeshAgent.desiredVelocity;

            _navMeshAgent.nextPosition = _moveCharacterModel.Transform.position;

            var input = new Vector2(desiredVelocity.x, desiredVelocity.z);

            var nextPositionHeight = _navMeshAgent.nextPosition.y - _moveCharacterModel.Transform.position.y;
            var jumpHeight = PositionHelper.CalculateJumpHeight(_stats.JumpForce, _moveCharacterModel.Rigidbody.mass);

            if (0 < nextPositionHeight && nextPositionHeight <= jumpHeight)
            {
                _mover.Jump(_stats.JumpForce);
            }

            _mover.Move(input, _stats.MoveSpeed, _stats.RotationSpeed);
        }
    }
}
