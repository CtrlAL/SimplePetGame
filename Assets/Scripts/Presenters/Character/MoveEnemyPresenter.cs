using Models;
using ScriptableObjects;
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

        private const float RepathInterval = 0.25f;
        private const float CornerReachThresholdSqr = 0.0625f;

        private float _repathTimer;
        private int _cornerIndex;

        public void Initialize()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;
            _cornerIndex = 1;
        }

        public void FixedTick()
        {
            if (!_navMeshAgent.enabled) return;

            _repathTimer -= Time.fixedDeltaTime;
            if (_repathTimer <= 0f)
            {
                _navMeshAgent.SetDestination(_playerProvider.Instance.transform.position);
                _repathTimer = RepathInterval;
                _cornerIndex = 1;
            }

            var path = _navMeshAgent.path;
            if (path == null || path.status != NavMeshPathStatus.PathComplete)
            {
                _mover.Move(Vector2.zero, 0f, _stats.RotationSpeed);
                _navMeshAgent.nextPosition = _moveCharacterModel.Transform.position;
                return;
            }

            var corners = path.corners;
            _cornerIndex = Mathf.Clamp(_cornerIndex, 1, Mathf.Max(1, corners.Length - 1));

            var pos = _moveCharacterModel.Transform.position;
            while (_cornerIndex < corners.Length &&
                   (pos - corners[_cornerIndex]).sqrMagnitude < CornerReachThresholdSqr)
            {
                _cornerIndex++;
            }

            if (_cornerIndex >= corners.Length)
            {
                _mover.Move(Vector2.zero, 0f, _stats.RotationSpeed);
            }
            else
            {
                var dir = corners[_cornerIndex] - pos;
                var input = new Vector2(dir.x, dir.z).normalized;
                _mover.Move(input, _stats.MoveSpeed, _stats.RotationSpeed);
            }

            _navMeshAgent.nextPosition = _moveCharacterModel.Transform.position;
        }
    }
}
