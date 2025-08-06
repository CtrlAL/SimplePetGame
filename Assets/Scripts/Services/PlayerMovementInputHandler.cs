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

        public PlayerMovementInputHandler(IPlayerInputProvider playerInputProvider, PlayerStatsSO playerStatsSO, CharacterFSM characterFSM, Rigidbody rigidbody)
        {
            _playerInputProvider = playerInputProvider;
            _characterFSM = characterFSM;
            _rigidbody = rigidbody;
            _stats = playerStatsSO;

            _playerInputProvider.InputActions.Enable();
            _playerInputProvider.Inputs.Jump.performed += PublishJump;
        }

        private void PublishMove(InputAction.CallbackContext context)
        {
            PublishMove();
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            PublishJump();
        }

        public void Dispose()
        {
            _playerInputProvider.Inputs.Jump.performed -= PublishJump;
            _playerInputProvider.Inputs.Jump.performed -= PublishMove;
            _playerInputProvider.InputActions.Disable();
        }

        public void MovePerFame()
        {
            if (_playerInputProvider.InputActions.Inputs.Move.IsPressed())
            {
                var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
                MoveEventPublisher.Instance.PublishMoveEvent(input, _characterFSM, _rigidbody, _stats.MoveSpeed);
            }
        }

        public void PublishMove()
        {
            var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
            MoveEventPublisher.Instance.PublishMoveEvent(input, _characterFSM, _rigidbody, _stats.MoveSpeed);
        }

        public void PublishJump()
        {
            MoveEventPublisher.Instance.PublishJumpEvent(_characterFSM, _rigidbody, _stats.JumpForce);
        }
    }
}

