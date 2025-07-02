using UnityEngine;

namespace Assets.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] EnemySpawner _enemySpawner;

        [SerializeField] PlayerMovementInputHandler _playerInput;

        private Kicker _kiker;
        private Mover _mover;
        private EnemyFactory _enemyFactory;

        public void Awake()
        {
            _kiker = new Kicker();
            _mover = new Mover();
            _enemyFactory = new EnemyFactory();
        }

        void FixedUpdate()
        {
            _enemyFactory.PublicUpdate();
            _playerInput.PublicUpdate();
        }
    }
}


