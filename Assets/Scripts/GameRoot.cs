using Services;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace GameRoot
{
    public class GameRoot : MonoBehaviour
    {
        [Header("Input Consumers")]
        [Inject] private IKiker _kiker;
        [Inject] private IMover _mover;

        [Header("Factory")]
        [Inject] 
        private IEnemyFactory _enemyFactory;

        [Header("Headnlers")]
        [Inject]
        private PlayerMovementInputHandler _playerMovementInputHandler;

        [Header("Effects")]
        [Inject]
        private DeathEffectPresenter _deathEffectPresenter;
        [Inject]
        private StunEffectPresenter _stunEffectPresenter;

        void FixedUpdate()
        {
            _enemyFactory.Tick();
            _playerMovementInputHandler.PublicUpdate();
        }
    }
}


