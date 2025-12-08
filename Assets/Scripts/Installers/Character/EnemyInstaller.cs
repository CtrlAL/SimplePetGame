using Presenters;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using Views;

namespace Services.Installers
{
    public class EnemyInstaller : CharacterInstaller
    {
        private enum EnemyType
        {
            Default,
            Big
        }

        [SerializeField] private EnemyType _enemyType;

        public override void InstallBindings()
        {
            BindStats();
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<EnemyKickZoneView>().FromComponentOnRoot().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemyKickPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<MoveEnemyPresenter>().AsSingle().NonLazy();

            base.InstallBindings();
        }

        private void BindStats()
        {
            switch (_enemyType)
            {
                case EnemyType.Default:
                    Container.Bind<AbstractStats>().To<EnemyStats>().AsSingle();
                    break;

                case EnemyType.Big:
                    Container.Bind<AbstractStats>().To<BigEnemyStats>().AsSingle();
                    break;
            }
        }
    }
}