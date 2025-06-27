using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputActions _inputActions;

        public void Awake()
        {
            _inputActions = PlayerInputProvider.GetInputActions();
            _inputActions.Enable();

            _inputActions.Inputs.Jump.performed += PublishJump;
        }

        public void PublicUpdate()
        {
            if (_inputActions.Inputs.Move.IsPressed() &&
                PlayerInstanse.Instance.TryGetComponent<CharacterStats>(out var stats))
            {
                var input = _inputActions.Inputs.Move.ReadValue<Vector2>();
                MoveEventPublisher.Instance.PublishMoveEvent(input, PlayerInstanse.Instance, stats.MoveSpeed);
            }
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            if (PlayerInstanse.Instance.TryGetComponent<CharacterStats>(out var stats))
            {
                MoveEventPublisher.Instance.PublishJumpEvent(PlayerInstanse.Instance, stats.JumpForce);
            }
        }

        public void OnDestroy()
        {
            _inputActions.Inputs.Jump.performed -= PublishJump;
            _inputActions.Disable();
        }
    }
}

