using ScriptableObjects;
using Services.EventTriggers;
using UnityEngine;

namespace Services.Installers
{
    public class EnemyInstaller : CharacterInstaller
    {
        [SerializeField]
        private EnemyStatsSO _enemyStatsSO;

        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_enemyStatsSO).AsSingle();
            Container.Bind<EnemyDelayedKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();
        }
    }
}
