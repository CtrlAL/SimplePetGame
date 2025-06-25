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
            if (_inputActions.Inputs.Move.IsPressed())
            {
                var input = _inputActions.Inputs.Move.ReadValue<Vector2>();
                MoveEventPublisher.Instance.PublishMoveEvent(input, PlayerInstanse.Instance);
            }
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            MoveEventPublisher.Instance.PublishJumpEvent(PlayerInstanse.Instance);
        }

        public void OnDestroy()
        {
            _inputActions.Inputs.Jump.performed -= PublishJump;
            _inputActions.Disable();
        }
    }
}

