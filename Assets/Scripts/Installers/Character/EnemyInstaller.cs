using Presenters;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Services.Installers
{
    public class EnemyInstaller : CharacterInstaller
    {
        [SerializeField]
        private EnemyStats _enemyStatsSO;

        public override void InstallBindings()
        {
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<AbstractStats>().ToSelf().FromInstance(_enemyStatsSO).AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyKickPresenter>().AsSingle().NonLazy();
            Container.Bind<EnemyKickZoneView>().FromComponentOnRoot().AsSingle();

            Container.BindInterfacesAndSelfTo<MoveEnemyPresenter>().AsSingle().NonLazy();

            base.InstallBindings();
        }
    }
}