using UnityEngine;

namespace Services.Interfaces
{
    public interface IEnemyFactory 
    {
        int TotalActive { get; }
        void DestroyEnemy(GameObject args);
        GameObject CreateEnemy();
    }
}
