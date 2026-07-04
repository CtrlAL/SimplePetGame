using Extensions;
using FSM;
using Helpers;
using Models;
using ScriptableObjects;
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

        public Subject<MoveCharacterModel> _onJumped = new();
        public Subject<MoveCharacterModel> _onMoved = new();

        IObservable<MoveCharacterModel> IMover.OnJumped => _onJumped;
        IObservable<MoveCharacterModel> IMover.OnMoved => _onMoved;

        public void Move(Vector2 input, float speed, float rotationSpeed)
        {
            if (_moveCharacterModel.Rigidbody == null || !_characterFSM.IsIdleState())
                return;

            var rb = _moveCharacterModel.Rigidbody;
            var maxDelta = _stats.Acceleration * Time.fixedDeltaTime;

            var desired = new Vector3(input.x, 0f, input.y);
            if (desired.sqrMagnitude > 1f) desired.Normalize();
            desired *= speed;

            var current = rb.velocity;
            var target = new Vector3(desired.x, current.y, desired.z);
            rb.velocity = Vector3.MoveTowards(current, target, maxDelta);

            if (desired.sqrMagnitude > 0.001f)
                Rotation(new Vector3(input.x, 0f, input.y), rotationSpeed);

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
                    rb.position = new Vector3(p.x, targetY, p.z);
            }
        }
    }
}
