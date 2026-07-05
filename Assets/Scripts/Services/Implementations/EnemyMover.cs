using Extensions;
using FSM;
using Helpers;
using Models;
using ScriptableObjects;
using Services.Helpers;
using Services.Interfaces;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Services
{
    public class EnemyMover : IMover
    {
        [Inject] private CharacterFSM _characterFSM;
        [Inject] private MoveCharacterModel _moveCharacterModel;
        [Inject] private AbstractStats _stats;

        private const float NavMeshSnapSearchRadius = 1.0f;
        private const float NavMeshSnapMaxDelta = 0.6f;
        private const float GroundCheckDistance = 1f;
        private const float ForwardRampDistance = 1f;
        private const float MaxSlopeAngle = 60f;

        public Subject<MoveCharacterModel> _onJumped = new();
        public Subject<MoveCharacterModel> _onMoved = new();

        IObservable<MoveCharacterModel> IMover.OnJumped => _onJumped;
        IObservable<MoveCharacterModel> IMover.OnMoved => _onMoved;

        public void Move(Vector2 input, float speed, float rotationSpeed)
        {
            if (_moveCharacterModel.Rigidbody == null || !_characterFSM.IsIdleState())
                return;

            var rb = _moveCharacterModel.Rigidbody;
            var tr = _moveCharacterModel.Transform;
            var maxDelta = _stats.Acceleration * Time.fixedDeltaTime;

            var movement = new Vector3(input.x, 0f, input.y);
            if (movement.sqrMagnitude > 1f) movement.Normalize();

            var groundHit = GroundChecker.TryGetSurfaceNormal(
                tr.position, GroundCheckDistance, out var normal, true);

            var forwardHit = GroundChecker.TryGetForwardNormal(
                movement, tr, ForwardRampDistance, MaxSlopeAngle, out var forwardNormal);

            if (forwardHit)
                normal = forwardNormal;

            var onSlope = (GroundChecker.IsCompletelyOffPlatform(_moveCharacterModel.BoxCollider, tr)
                          || forwardHit) && groundHit;

            var moveDir = onSlope
                ? Vector3.ProjectOnPlane(movement, normal)
                : movement;

            if (onSlope)
            {
                var target = moveDir * speed;
                rb.velocity = Vector3.MoveTowards(rb.velocity, target, maxDelta);
            }
            else
            {
                var current = rb.velocity;
                var target = new Vector3(moveDir.x * speed, current.y, moveDir.z * speed);
                rb.velocity = Vector3.MoveTowards(current, target, maxDelta);
            }

            if (movement.sqrMagnitude > 0.001f)
                Rotation(new Vector3(input.x, 0f, input.y), rotationSpeed);

            if (!onSlope)
                SnapToNavMeshSurface(rb);

            _onMoved.OnNext(_moveCharacterModel);
        }

        public void Jump(float jumpForce)
        {
            if (GameHelpers.IsGrounded(_moveCharacterModel.Rigidbody.gameObject)
                && _characterFSM.IsIdleState())
            {
                _moveCharacterModel.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _onJumped.OnNext(_moveCharacterModel);
            }
        }

        public void Rotation(Vector3 movement, float rotationSpeed)
        {
            var targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            var rb = _moveCharacterModel.Rigidbody;

            rb.MoveRotation(Quaternion.Slerp(
                rb.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            ));
        }

        private void SnapToNavMeshSurface(Rigidbody rb)
        {
            if (_moveCharacterModel.BoxCollider == null) return;

            var tr = _moveCharacterModel.Transform;
            var halfHeight = _moveCharacterModel.BoxCollider.bounds.extents.y;

            if (NavMesh.SamplePosition(
                    tr.position + Vector3.down * 0.5f,
                    out var hit,
                    NavMeshSnapSearchRadius,
                    NavMesh.AllAreas))
            {
                var targetY = hit.position.y + halfHeight;
                var p = rb.position;
                if (Mathf.Abs(p.y - targetY) < NavMeshSnapMaxDelta)
                {
                    rb.position = new Vector3(p.x, targetY, p.z);
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                }
            }
        }
    }
}
