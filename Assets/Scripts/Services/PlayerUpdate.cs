using Services.Interfaces;
using Zenject;

namespace Services
{
    public class PlayerUpdate : IFixedTickable
    {
        [Inject]
        private IPlayerMovementInputHandler _playerMovementInputHandler;

        public void FixedTick()
        {
            _playerMovementInputHandler.MovePerFame();
        }
    }
}
