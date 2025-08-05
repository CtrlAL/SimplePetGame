using Services.Interfaces;
using Zenject;

namespace Services
{
    public class PlayerInputProvider : IPlayerInputProvider
    {
        [Inject] 
        private PlayerInputActions _inputActions;
        public PlayerInputActions InputActions => _inputActions;
        public PlayerInputActions.InputsActions Inputs => _inputActions.Inputs;

        private PlayerInputProvider()
        {
            _inputActions.Inputs.Enable();
        }

        public void Dispose()
        {
            _inputActions.Dispose();
        }
    }
}