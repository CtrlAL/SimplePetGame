
using System;

namespace Services.Interfaces
{
    public interface IPlayerInputProvider : IDisposable
    {
        public PlayerInputActions InputActions { get; }
        public PlayerInputActions.InputsActions Inputs => InputActions.Inputs;
    }
}