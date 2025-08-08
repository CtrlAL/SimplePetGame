using Assets.Scripts.Services;
using FSM;
using Presenters;
using ScriptableObjects;
using Services.Interfaces;
using Services.Sound;
using UnityEngine;
using Views;
using Zenject;

namespace Services.Installers
{
    public partial class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelSettings _levelSettings;

        [SerializeField]
        private PoolingSettings _poolingSettings;

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
            InstallSO();
            InstallPlayerInputs();
            InstallServices();
            InstallPresenters();
            InstallViews();
            InstallTickable();
            InstallSound();
        }

        private void InstallServices()
        {
            Container.Bind<StateMachine>().ToSelf().AsSingle();

            Container.Bind<IMover>().To<Mover>().AsSingle();
            Container.Bind<IKiker>().To<Kicker>().AsSingle();
            Container.Bind<IStuner>().To<Stuner>().AsSingle();

            Container.Bind<IEnemyPool>().To<EnemyPool>()
                .AsSingle();

            Container.Bind<IDeathEffectPool>().To<DeathEffectPool>()
                .AsSingle();

            Container.Bind<IStunEffectPool>().To<StunEffectPool>()
                .AsSingle();

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
            Container.BindInterfacesAndSelfTo<SoundEventRouter>().AsSingle();
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<CharacterDeathPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacerRespawnPresenter>().AsSingle().NonLazy();
        }

        private void InstallTickable()
        {
            Container.Bind<ITickable>().To<SceneUpdate>().AsSingle();
        }

        private void InstallViews()
        {
            Container.Bind<DeathEffectView>().ToSelf().AsSingle();
            Container.Bind<StunEffectPresenter>().ToSelf().AsSingle();
            Container.Bind<PlayerSpawnPoint>().FromComponentInHierarchy().AsSingle();

            Container.Bind<RespawnColiderView>().FromComponentsInHierarchy().AsSingle();
        }

        private void InstallSO()
        {
            Container.Bind<LevelSettings>().ToSelf().FromInstance(_levelSettings).AsSingle();
            Container.Bind<PoolingSettings>().ToSelf().FromInstance(_poolingSettings).AsSingle();
            Container.Bind<PlayerStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();
            Container.Bind<EnemyStatsSO>().ToSelf().FromInstance(_enemyStatsSO).AsSingle();
            Container.Bind<KickImpactSettigns>().ToSelf().FromInstance(_kickImpactSettigns).AsSingle();
            Container.Bind<CharacterVFX>().ToSelf().FromInstance(_characterVFX).AsSingle();
            Container.Bind<EnemyLibrary>().ToSelf().FromInstance(_enemyLibrary).AsSingle();
            Container.Bind<ThrowableInteractionSettingsSO>().ToSelf().FromInstance(_throwableInteractionSettingsSO).AsSingle();
        }
    }
}
