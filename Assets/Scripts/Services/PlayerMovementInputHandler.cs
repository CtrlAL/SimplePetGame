using FSM;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Services
{
    public class PlayerMovementInputHandler : IDisposable, ITickable
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

        public void Tick()
        {
            if (_playerInputProvider.InputActions.Inputs.Move.IsPressed())
            {
                var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
                MoveEventPublisher.Instance.PublishMoveEvent(input, _characterFSM, _rigidbody, _stats.MoveSpeed);
            }
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            var fsm = PlayerInstanseHandler.Instance.GetComponent<CharacterFSM>();
            var rb = PlayerInstanseHandler.Instance.GetComponent<Rigidbody>();
            MoveEventPublisher.Instance.PublishJumpEvent(fsm, rb, _stats.JumpForce);
        }

        public void Dispose()
        {
            _playerInputProvider.Inputs.Jump.performed -= PublishJump;
            _playerInputProvider.InputActions.Disable();
        }
    }
}

