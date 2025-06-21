using UnityEngine;

public class PlayerInput : MonoBehaviour, IMoveInput
{
    private PlayerInputActions _inputActions;

    public Vector2 MoveInput => _inputActions.Inputs.Move.ReadValue<Vector2>();

    public bool JumpPerformed => _inputActions.Inputs.Jump.IsPressed();

    public void Awake()
    {
        _inputActions = new PlayerInputActions();
        _inputActions.Enable();
    }

    public void OnDestroy()
    {
        _inputActions.Disable();
    }
}
