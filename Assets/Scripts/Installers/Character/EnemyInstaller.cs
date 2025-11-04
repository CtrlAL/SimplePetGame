using Presenters;
using ScriptableObjects;
using UnityEngine.AI;
using Views;

namespace Services.Installers
{
    public class EnemyInstaller : CharacterInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<AbstractStats>().To<EnemyStats>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyKickPresenter>().AsSingle().NonLazy();
            Container.Bind<EnemyKickZoneView>().FromComponentOnRoot().AsSingle();

            Container.BindInterfacesAndSelfTo<MoveEnemyPresenter>().AsSingle().NonLazy();

            base.InstallBindings();
        }
    }
}