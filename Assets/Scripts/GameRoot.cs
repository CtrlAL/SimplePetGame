using UnityEngine;

namespace Assets.Scripts
{
    public class GameRoot : MonoBehaviour
    {
        [SerializeField] EnemySpawner _enemySpawner;

        void Update()
        {
            _enemySpawner.PublicUpdate();
        }
    }
}


