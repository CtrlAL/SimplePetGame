using Assets.Scripts.FSM;
using Assets.Scripts.ScriptableObjects;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerMovementInputHandler : IDisposable
    {
        private PlayerStatsSO _stats;

        private PlayerInputActions _inputActions;

        public PlayerMovementInputHandler()
        {
            _inputActions = PlayerInputProvider.Instance.Actions;
            _inputActions.Enable();
            _inputActions.Inputs.Jump.performed += PublishJump;
            _stats = Resources.Load<PlayerStatsSO>("ScriptableObjects/PlayerStats");
        }

        public void PublicUpdate()
        {
            if (_inputActions.Inputs.Move.IsPressed())
            {
                var fsm = PlayerInstanseHandler.Instance.GetComponent<CharacterFSM>();
                var rb = PlayerInstanseHandler.Instance.GetComponent<Rigidbody>();

                var input = _inputActions.Inputs.Move.ReadValue<Vector2>();
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
            _inputActions.Inputs.Jump.performed -= PublishJump;
            _inputActions.Disable();
        }
    }
}

