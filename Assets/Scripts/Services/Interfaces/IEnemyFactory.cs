using System;
using UnityEngine;
using Zenject;

namespace Services.Interfaces
{
    public interface IEnemyFactory : ITickable, IDisposable
    {
        void DestroyEnemy(object sender, GameObject args);
        void CreateEnemy();
    }
}
