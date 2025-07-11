using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private EffectsFactory2D _effectsFactory;

        [Header("Input Consumers")]
        private Kicker _kiker;
        private Mover _mover;

        [Header("Facotys")]
        private EnemyFactory _enemyFactory;

        [Header("Headnlers")]
        private PlayerMovementInputHandler _playerMovementInputHandler;

        [Header("Effects")]
        private DeathEffectPresenter _deathEffectPresenter;
        private StunEffectPresenter _stunEffectPresenter;

        public void Awake()
        {
            _kiker = new Kicker();
            _mover = new Mover();
            _enemyFactory = new EnemyFactory();
            _playerMovementInputHandler = new PlayerMovementInputHandler();
            _deathEffectPresenter = new DeathEffectPresenter();
            _stunEffectPresenter = new StunEffectPresenter();
        }

        void FixedUpdate()
        {
            _enemyFactory.PublicUpdate();
            _playerMovementInputHandler.PublicUpdate();
            _effectsFactory?.PublicUpdate();
        }
    }
}


