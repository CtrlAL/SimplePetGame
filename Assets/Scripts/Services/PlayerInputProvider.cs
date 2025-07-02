using System;

namespace Assets.Scripts
{
    public class PlayerInputProvider : IDisposable
    {
        private static readonly Lazy<PlayerInputProvider> _instance = new(() => new PlayerInputProvider());
        public static PlayerInputProvider Instance => _instance.Value;

        public PlayerInputActions Actions { get; }
        public PlayerInputActions.InputsActions Inputs => Actions.Inputs;

        private PlayerInputProvider()
        {
            Actions = new PlayerInputActions();
            Actions.Inputs.Enable();
        }

        public void Dispose()
        {
            Actions.Dispose();
        }
    }
    
}