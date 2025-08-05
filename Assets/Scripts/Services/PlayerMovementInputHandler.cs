using FSM;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services
{
    public class PlayerMovementInputHandler : IDisposable
    {
        private PlayerStatsSO _stats;

        private IPlayerInputProvider _playerInputProvider;

        public PlayerMovementInputHandler(IPlayerInputProvider playerInputProvider, PlayerStatsSO playerStatsSO)
        {
            _playerInputProvider = playerInputProvider;
            _playerInputProvider.InputActions.Enable();
            _playerInputProvider.Inputs.Jump.performed += PublishJump;
            _stats = playerStatsSO;
        }

        public void PublicUpdate()
        {
            if (_playerInputProvider.InputActions.Inputs.Move.IsPressed())
            {
                var fsm = PlayerInstanseHandler.Instance.GetComponent<CharacterFSM>();
                var rb = PlayerInstanseHandler.Instance.GetComponent<Rigidbody>();

                var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
                MoveEventPublisher.Instance.PublishMoveEvent(input, fsm, rb, _stats.MoveSpeed);
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

