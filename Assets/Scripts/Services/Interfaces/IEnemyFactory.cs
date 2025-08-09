using UnityEngine;

namespace Services.Interfaces
{
    public interface IEnemyFactory 
    {
        void DestroyEnemy(GameObject args);
        void CreateEnemy();
    }
}
