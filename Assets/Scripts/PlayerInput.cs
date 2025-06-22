using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerInput : MonoBehaviour, IMoveInput
    {
        private PlayerInputActions _inputActions;
        public Vector2 MoveInput => _inputActions.Inputs.Move.ReadValue<Vector2>();

        public bool JumpPerformed => _inputActions.Inputs.Jump.IsPressed();

        public void Awake()
        {
            _inputActions = PlayerInputProvider.GetInputActions();
            _inputActions.Enable();
        }

        public void OnDestroy()
        {
            _inputActions.Disable();
        }
    }
}

