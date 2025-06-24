using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] MoveEventPublisher _moveEventPublisher;

        private PlayerInputActions _inputActions;

        public void Awake()
        {
            _inputActions = PlayerInputProvider.GetInputActions();
            _inputActions.Enable();

            _inputActions.Inputs.Jump.performed += PublishJump;
        }

        public void FixedUpdate()
        {
            if (_inputActions.Inputs.Move.IsPressed())
            {
                var input = _inputActions.Inputs.Move.ReadValue<Vector2>();
                _moveEventPublisher.PublishMoveEvent(input, Helpers.FindPlayer());
            }
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            _moveEventPublisher.PublishJumpEvent(Helpers.FindPlayer());
        }

        public void OnDestroy()
        {
            _inputActions.Inputs.Jump.performed -= PublishJump;
            _inputActions.Disable();
        }
    }
}

