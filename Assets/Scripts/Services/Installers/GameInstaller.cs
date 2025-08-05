using FSM;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services.Installers
{
    public partial class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerStatsSO _playerStatsSO;

        [SerializeField]
        private EnemyStatsSO _enemyStatsSO;

        [SerializeField]
        private KickImpactSettigns _kickImpactSettigns;

        [SerializeField]
        private CharacterVFX _characterVFX;

        [SerializeField]
        private EnemyLibrary _enemyLibrary;

        public override void InstallBindings()
        {
            InstallPlayerInputs();
            InstallServices();

            InstallModels();
            InstallViews();
            InstallPresenters();

            InstallSO();
        }

        private void InstallServices()
        {
            Container.Bind<StateMachine>().ToSelf().AsSingle();

            Container.Bind<IMover>().To<Mover>().AsSingle();
            Container.Bind<IKiker>().To<Kicker>().AsSingle();

            Container.Bind<CoroutineRunner>().ToSelf()
                .FromNewComponentOnNewGameObject()
                .AsSingle()
                .NonLazy();

            Container.Bind<IEnemyFactory>().To<EnemyFactory>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallPlayerInputs()
        {
            Container.Bind<IPlayerInputProvider>().To<PlayerInputProvider>().AsSingle().NonLazy();
            Container.Bind<PlayerInputActions>().ToSelf().FromInstance(new PlayerInputActions()).AsSingle();
        }


        private void InstallModels()
        {
        }

        private void InstallPresenters()
        {
        }

        private void InstallViews()
        {
            Container.Bind<DeathEffectPresenter>().ToSelf().AsSingle();
            Container.Bind<StunEffectPresenter>().ToSelf().AsSingle();
        }

        private void InstallSO()
        {
            Container.Bind<PlayerStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();
            Container.Bind<EnemyStatsSO>().ToSelf().FromInstance(_enemyStatsSO).AsSingle();
            Container.Bind<KickImpactSettigns>().ToSelf().FromInstance(_kickImpactSettigns).AsSingle();
            Container.Bind<CharacterVFX>().ToSelf().FromInstance(_characterVFX).AsSingle();
            Container.Bind<EnemyLibrary>().ToSelf().FromInstance(_enemyLibrary).AsSingle();
        }
    }
}
