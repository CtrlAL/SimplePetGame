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

        [Header("Facotys")]
        private EnemyFactory _enemyFactory;

        [Header("Headnlers")]
        private PlayerMovementInputHandler _playerMovementInputHandler;

        [Header("Effects")]
        private DeathEffectPresenter _deathEffectPresenter;
        private StunEffectPresenter _stunEffectPresenter;

        public void Awake()
        {
            _enemyFactory = new EnemyFactory();
            _playerMovementInputHandler = new PlayerMovementInputHandler();
            _deathEffectPresenter = new DeathEffectPresenter();
            _stunEffectPresenter = new StunEffectPresenter();
        }

        void FixedUpdate()
        {
            _enemyFactory.PublicUpdate();
            _playerMovementInputHandler.PublicUpdate();
        }
    }
}


