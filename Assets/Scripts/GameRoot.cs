using UnityEngine;

namespace Assets.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] EnemySpawner _enemySpawner;

        [SerializeField] PlayerMovementInputHandler _playerInput;

        void FixedUpdate()
        {
            _enemySpawner.PublicUpdate();
            _playerInput.PublicUpdate();
        }
    }
}


