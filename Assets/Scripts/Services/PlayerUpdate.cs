using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class PlayerUpdate : ITickable
    {
        [Inject]
        private IPlayerMovementInputHandler _playerMovementInputHandler { get; set; }

        public void Tick()
        {
            Debug.Log("Тикаю");
            _playerMovementInputHandler.MovePerFame();
        }
    }
}
