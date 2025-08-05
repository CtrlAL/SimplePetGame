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

        [Header("Headnlers")]
        [Inject]
        private PlayerMovementInputHandler _playerMovementInputHandler;

        [Header("Effects")]
        private DeathEffectPresenter _deathEffectPresenter;
        private StunEffectPresenter _stunEffectPresenter;

        public void Awake()
        {
            _deathEffectPresenter = new DeathEffectPresenter();
            _stunEffectPresenter = new StunEffectPresenter();
        }

        void FixedUpdate()
        {
            _playerMovementInputHandler.PublicUpdate();
        }
    }
}


