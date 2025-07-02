using UnityEngine;

namespace Assets.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        [Header("Input Consumers")]
        private Kicker _kiker;
        private Mover _mover;

        [Header("Facotys")]
        private EnemyFactory _enemyFactory;

        [Header("Headnlers")]
        private PlayerMovementInputHandler _playerMovementInputHandler;

        public void Awake()
        {
            _kiker = new Kicker();
            _mover = new Mover();
            _enemyFactory = new EnemyFactory();
            _playerMovementInputHandler = new PlayerMovementInputHandler();
        }

        void FixedUpdate()
        {
            _enemyFactory.PublicUpdate();
            _playerMovementInputHandler.PublicUpdate();
        }
    }
}


