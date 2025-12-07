using Extensions;
using FSM;
using Helpers;
using Models;
using Services.Helpers;
using Services.Interfaces;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services
{
    public class Mover : IMover
    {
        [Inject] CharacterFSM _characterFSM;

        [Inject] MoveCharacterModel _moveCharacterModel;

        public Subject<MoveCharacterModel> _onJumped = new();

        public Subject<MoveCharacterModel> _onMoved = new();

        IObservable<MoveCharacterModel> IMover.OnJumped => _onJumped;

        IObservable<MoveCharacterModel> IMover.OnMoved => _onMoved;

        private const float _distanseToGround = 1f;
        private const float _distanseToForwardRamp = 1f;

        public void Move(Vector2 input, float speed, float rotationSpeed)
        {
            if (_moveCharacterModel.Rigidbody != null && _characterFSM.IsIdleState())
            {
                var movement = new Vector3(input.x, 0f, input.y);
                var groudNormalResult = GroundChecker.TryGetSurfaceNormal(_moveCharacterModel.Transform.position, _distanseToGround, out var normal, true);
                var forwardNormalResult = GroundChecker.TryGetForwardNormal(movement, _moveCharacterModel.Transform, _distanseToForwardRamp, 60f, out var forwardNormal);

                var bounds = _moveCharacterModel.BoxCollider.bounds;

                if (forwardNormalResult)
                {
                    normal = forwardNormal;
                }

                var movementOnSlope = GroundChecker.IsCompletelyOffPlatform(_moveCharacterModel.BoxCollider, _moveCharacterModel.Transform) || forwardNormalResult
                    ? Vector3.ProjectOnPlane(movement, normal)
                    : movement;

                _moveCharacterModel.Rigidbody.AddForce(movementOnSlope * speed, ForceMode.Force);
                Rotation(movement, rotationSpeed);
                _onMoved.OnNext(_moveCharacterModel);
            }
        }

        public void Jump(float jumpForce)
        {
            if (GameHelpers.IsGrounded(_moveCharacterModel.Rigidbody.gameObject) && _characterFSM.IsIdleState())
            {
                _moveCharacterModel.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                _onJumped.OnNext(_moveCharacterModel);
            }
        }

        public void Rotation(Vector3 movement, float rotationSpeed)
        {
            var targetRotation = Quaternion.LookRotation(movement, Vector3.up);

            _moveCharacterModel.Transform.rotation = Quaternion.Slerp(
                _moveCharacterModel.Transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}