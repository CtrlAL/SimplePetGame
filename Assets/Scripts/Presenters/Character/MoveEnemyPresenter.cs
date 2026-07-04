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

        private float _jumpCooldown;
        private const float JumpCooldownSeconds = 0.5f;

        public void Initialize()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
        }

        public void FixedTick()
        {
            if (!_navMeshAgent.enabled) return;

            _jumpCooldown -= Time.fixedDeltaTime;

            var target = _playerProvider.Instance.transform.position;

            _navMeshAgent.SetDestination(target);

            var desiredVelocity = _navMeshAgent.desiredVelocity;

            var input = new Vector2(desiredVelocity.x, desiredVelocity.z);

            var nextPositionHeight = _navMeshAgent.nextPosition.y - _moveCharacterModel.Transform.position.y;
            var jumpHeight = PositionHelper.CalculateJumpHeight(_stats.JumpForce, _moveCharacterModel.Rigidbody.mass);

            if (_jumpCooldown <= 0f && 0 < nextPositionHeight && nextPositionHeight <= jumpHeight)
            {
                _mover.Jump(_stats.JumpForce);
                _jumpCooldown = JumpCooldownSeconds;
            }

            _navMeshAgent.nextPosition = _moveCharacterModel.Transform.position;

            _mover.Move(input, _stats.MoveSpeed, _stats.RotationSpeed);
        }
    }
}
