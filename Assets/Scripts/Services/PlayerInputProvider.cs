using Services.Interfaces;

namespace Services
{
    public class PlayerInputProvider : IPlayerInputProvider
    {
        private PlayerInputActions _inputActions;
        public PlayerInputActions InputActions => _inputActions;
        public PlayerInputActions.InputsActions Inputs => _inputActions.Inputs;

        public PlayerInputProvider(PlayerInputActions inputActions)
        {
            _inputActions = inputActions;
            _inputActions.Inputs.Enable();
        }

        public void Dispose()
        {
            _inputActions.Dispose();
        }
    }
}