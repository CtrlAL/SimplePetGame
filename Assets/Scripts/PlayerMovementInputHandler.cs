using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Enums;

namespace Assets.Scripts
{
    public class PlayerMovementInputHandler : MonoBehaviour
    {
        [SerializeField] PlayerStatsSO _stats;

        private PlayerInputActions _inputActions;

        public void Awake()
        {
            _inputActions = PlayerInputProvider.Inputs;
            _inputActions.Enable();
            _inputActions.Inputs.Jump.performed += PublishJump;
        }

        public void PublicUpdate()
        {
            if (_inputActions.Inputs.Move.IsPressed())
            {
                var input = _inputActions.Inputs.Move.ReadValue<Vector2>();
                MoveEventPublisher.Instance.PublishMoveEvent(input, PlayerInstanse.Instance, _stats.MoveSpeed);
            }
        }

        private void PublishJump(InputAction.CallbackContext context)
        {
            MoveEventPublisher.Instance.PublishJumpEvent(PlayerInstanse.Instance, _stats.JumpForce);
            SoundEventPublisher.Instance.PlaySound(SoundType.Jump, 1);
        }

        public void OnDestroy()
        {
            _inputActions.Inputs.Jump.performed -= PublishJump;
            _inputActions.Disable();
        }
    }
}

