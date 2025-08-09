using System;
using UnityEngine;
using Zenject;

namespace Services.Interfaces
{
    public interface IEnemyFactory 
    {
        void DestroyEnemy(GameObject args);
        void CreateEnemy();
    }
}
