using FSM;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services
{
    public class PlayerMovementInputHandler : IPlayerMovementInputHandler
    {
        private PlayerStatsSO _stats;

        private CharacterFSM _characterFSM;

        private Rigidbody _rigidbody;

        private IPlayerInputProvider _playerInputProvider;

        private MoveEventPublisher _moveEventPublisher;

        public PlayerMovementInputHandler(IPlayerInputProvider playerInputProvider, 
            PlayerStatsSO playerStatsSO, 
            CharacterFSM characterFSM, 
            Rigidbody rigidbody, 
            MoveEventPublisher moveEventPublisher)
        {
            _moveEventPublisher = moveEventPublisher;

            _playerInputProvider = playerInputProvider;
            _characterFSM = characterFSM;
            _rigidbody = rigidbody;
            _stats = playerStatsSO;

            _playerInputProvider.InputActions.Enable();
            _playerInputProvider.Inputs.Jump.performed += PublishJump;
            _moveEventPublisher = moveEventPublisher;
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            PublishJump();
        }

        public void Dispose()
        {
            _playerInputProvider.Inputs.Jump.performed -= PublishJump;
            _playerInputProvider.InputActions.Disable();
        }

        public void MovePerFame()
        {
            if (_playerInputProvider.InputActions.Inputs.Move.IsPressed())
            {
                var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
                _moveEventPublisher.PublishMoveEvent(input, _characterFSM, _rigidbody, _stats.MoveSpeed, _stats.RotationSpeed);
            }
        }

        public void PublishMove()
        {
            var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
            _moveEventPublisher.PublishMoveEvent(input, _characterFSM, _rigidbody, _stats.MoveSpeed, _stats.RotationSpeed);
        }

        public void PublishJump()
        {
            _moveEventPublisher.PublishJumpEvent(_characterFSM, _rigidbody, _stats.JumpForce);
        }
    }
}

