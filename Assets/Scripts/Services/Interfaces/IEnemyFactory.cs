using System;
using UnityEngine;
using Zenject;

namespace Services.Interfaces
{
    public interface IEnemyFactory : ITickable, IDisposable
    {
        void DestroyEnemy(GameObject args);
        void CreateEnemy();
    }
}
