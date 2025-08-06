using System;

namespace Services.Interfaces
{
    public interface IPlayerMovementInputHandler : IDisposable
    {
        void PublishMove();
        void MovePerFame();
        void PublishJump();
    }
}
