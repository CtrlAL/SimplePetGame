using Assets.Scripts.Presenters;
using ScriptableObjects;
using Services.EventTriggers;
using UnityEngine;
using UnityEngine.AI;

namespace Services.Installers
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(EnemyDelayedKickTrigger))]
    public class EnemyInstaller : CharacterInstaller
    {
        [SerializeField]
        private EnemyStatsSO _enemyStatsSO;

        public override void InstallBindings()
        {
            Container.Bind<NavMeshAgent>().FromComponentOnRoot().AsSingle();
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_enemyStatsSO).AsSingle();
            Container.Bind<EnemyDelayedKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<MoveEnemyPresenter>().AsSingle().NonLazy();

            base.InstallBindings();
        }
    }
}

