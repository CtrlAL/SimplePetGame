using FSM;
using ScriptableObjects;
using Services.Interfaces;
using Services.Sound;
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
        private ThrowableInteractionSettingsSO _throwableInteractionSettingsSO;

        [SerializeField]
        private KickImpactSettigns _kickImpactSettigns;

        [SerializeField]
        private CharacterVFX _characterVFX;

        [SerializeField]
        private EnemyLibrary _enemyLibrary;

        public override void InstallBindings()
        {
            InstallPlayerInputs();
            InstallServices();;
            InstallViews();
            InstallSO();
            InstallTickable();
            InstallSound();
        }

        private void InstallServices()
        {
            Container.Bind<StateMachine>().ToSelf().AsSingle();

            Container.Bind<IMover>().To<Mover>().AsSingle();
            Container.Bind<IKiker>().To<Kicker>().AsSingle();
            Container.Bind<IStuner>().To<Stuner>().AsSingle();

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
            Container.Bind<PlayerInputActions>().ToSelf().AsSingle();
        }

        private void InstallSound()
        {
            Container.Bind<SoundManager>().ToSelf().FromComponentInHierarchy().AsSingle();
            Container.Bind<BackgroundMusicPlayer>().ToSelf().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SoundEventRouter>().AsSingle().NonLazy();
        }

        private void InstallTickable()
        {
            Container.Bind<ITickable>().To<SceneUpdate>().AsSingle();
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
            Container.Bind<ThrowableInteractionSettingsSO>().ToSelf().FromInstance(_throwableInteractionSettingsSO).AsSingle();
        }
    }
}
