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
        private const float LinkTraversalDuration = 0.6f;
        private const float LinkArcHeight = 1.5f;

        private float _repathTimer;
        private int _cornerIndex;

        private bool _traversingLink;
        private OffMeshLinkData _currentLink;
        private Vector3 _linkStartPos;
        private float _linkTimer;

        public void Initialize()
        {
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.autoTraverseOffMeshLink = false;
            _cornerIndex = 1;
        }

        public void FixedTick()
        {
            if (!_navMeshAgent.enabled) return;

            if (_traversingLink)
            {
                UpdateLinkTraversal();
                return;
            }

            _repathTimer -= Time.fixedDeltaTime;
            if (_repathTimer <= 0f)
            {
                _navMeshAgent.SetDestination(_playerProvider.Instance.transform.position);
                _repathTimer = RepathInterval;
                _cornerIndex = 1;
            }

            if (_navMeshAgent.isOnOffMeshLink)
            {
                StartLinkTraversal(_navMeshAgent.currentOffMeshLinkData);
                return;
            }

            var path = _navMeshAgent.path;
            if (path == null || path.status != NavMeshPathStatus.PathComplete)
            {
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

        private void StartLinkTraversal(OffMeshLinkData link)
        {
            _currentLink = link;
            _linkStartPos = _moveCharacterModel.Transform.position;
            _traversingLink = true;
            _linkTimer = 0f;

            var rb = _moveCharacterModel.Rigidbody;
            rb.velocity = Vector3.zero;

            var dir = link.endPos - _linkStartPos;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                rb.MoveRotation(Quaternion.LookRotation(dir));
            }
        }

        private void UpdateLinkTraversal()
        {
            _linkTimer += Time.fixedDeltaTime;
            var t = Mathf.Clamp01(_linkTimer / LinkTraversalDuration);

            var rb = _moveCharacterModel.Rigidbody;
            var pos = Vector3.Lerp(_linkStartPos, _currentLink.endPos, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * LinkArcHeight;

            rb.velocity = Vector3.zero;
            rb.MovePosition(pos);

            if (t >= 1f)
            {
                _navMeshAgent.CompleteOffMeshLink();
                _traversingLink = false;
                _navMeshAgent.nextPosition = _currentLink.endPos;
            }
        }
    }
}
