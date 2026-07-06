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
        private const float MinLinkTraversalDuration = 0.2f;
        private const float MaxLinkTraversalDuration = 0.8f;
        private const float MinLinkArcHeight = 0.2f;
        private const float MaxLinkArcHeight = 2f;
        private const float ArcHeightPadding = 0.2f;
        private const float DurationPerDistance = 0.4f;
        private const float LinkYThreshold = 0.5f;
        private const float LinkXYThreshold = 1.5f;

        private float _repathTimer;
        private int _cornerIndex;

        private bool _traversingLink;
        private Vector3 _linkStartPos;
        private Vector3 _linkEndPos;
        private float _linkTimer;
        private float _linkDuration;
        private float _linkArcHeight;

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

            _navMeshAgent.nextPosition = _moveCharacterModel.Transform.position;

            _repathTimer -= Time.fixedDeltaTime;
            if (_repathTimer <= 0f)
            {
                _navMeshAgent.SetDestination(_playerProvider.Instance.transform.position);
                _repathTimer = RepathInterval;
                _cornerIndex = 1;
            }

            if (_navMeshAgent.isOnOffMeshLink)
            {
                var linkData = _navMeshAgent.currentOffMeshLinkData;
                StartLinkTraversal(linkData.startPos, linkData.endPos);
                return;
            }

            var path = _navMeshAgent.path;
            if (path == null || path.status != NavMeshPathStatus.PathComplete)
            {
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
                return;
            }

            var nextCorner = corners[_cornerIndex];
            var toCorner = nextCorner - pos;
            if (Mathf.Abs(toCorner.y) > LinkYThreshold
                && new Vector2(toCorner.x, toCorner.z).sqrMagnitude < LinkXYThreshold * LinkXYThreshold)
            {
                StartLinkTraversal(pos, nextCorner);
                return;
            }

            var input = new Vector2(toCorner.x, toCorner.z).normalized;
            _mover.Move(input, _stats.MoveSpeed, _stats.RotationSpeed);
        }

        private void StartLinkTraversal(Vector3 startPos, Vector3 endPos)
        {
            _linkStartPos = startPos;
            _linkEndPos = endPos;
            _traversingLink = true;
            _linkTimer = 0f;

            var distance = Vector3.Distance(startPos, endPos);
            var heightDiff = Mathf.Abs(endPos.y - startPos.y);
            _linkDuration = Mathf.Clamp(distance * DurationPerDistance, MinLinkTraversalDuration, MaxLinkTraversalDuration);
            _linkArcHeight = Mathf.Clamp(heightDiff + ArcHeightPadding, MinLinkArcHeight, MaxLinkArcHeight);

            var rb = _moveCharacterModel.Rigidbody;
            rb.velocity = Vector3.zero;

            var dir = endPos - startPos;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                rb.MoveRotation(Quaternion.LookRotation(dir));
            }
        }

        private void UpdateLinkTraversal()
        {
            _linkTimer += Time.fixedDeltaTime;
            var t = Mathf.Clamp01(_linkTimer / _linkDuration);

            var rb = _moveCharacterModel.Rigidbody;
            var pos = Vector3.Lerp(_linkStartPos, _linkEndPos, t);
            pos.y += Mathf.Sin(t * Mathf.PI) * _linkArcHeight;

            rb.velocity = Vector3.zero;
            rb.MovePosition(pos);

            if (t >= 1f)
            {
                if (_navMeshAgent.isOnOffMeshLink)
                    _navMeshAgent.CompleteOffMeshLink();
                _traversingLink = false;
                _cornerIndex++;
                _navMeshAgent.nextPosition = _linkEndPos;
            }
        }
    }
}
