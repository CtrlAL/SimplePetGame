using Presenters;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using Services.Sound;
using UnityEngine;
using Views;
using Zenject;

namespace Services.Installers
{
    public partial class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private LevelSettings _levelSettings;

        [SerializeField] private PoolingSettings _poolingSettings;

        [SerializeField] private PlayerStatsSO _playerStatsSO;

        [SerializeField] private EnemyStatsSO _enemyStatsSO;

        [SerializeField] private ThrowableInteractionSettingsSO _throwableInteractionSettingsSO;

        [SerializeField] private KickImpactSettigns _kickImpactSettigns;

        [SerializeField] private CharacterVFX _characterVFX;

        [SerializeField] private EnemyLibrary _enemyLibrary;

        public override void InstallBindings()
        {
            InstallEvents();
            InstallSO();
            InstallPlayerInputs();
            InstallServices();
            InstallPresenters();
            InstallViews();
            InstallModels();
            InstallSound();
        }

        private void InstallEvents()
        {
            Container.Bind<SoundEventPublisher>().ToSelf().AsSingle();
        }

        private void InstallServices()
        {
            Container.Bind<IEnemyPool>().To<EnemyPool>()
                .AsSingle();

            Container.Bind<IDeathEffectPool>().To<DeathEffectPool>()
                .AsSingle();

            Container.Bind<IStunEffectPool>().To<StunEffectPool>()
                .AsSingle();

            Container.BindInterfacesTo<EnemyFactory>()
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
            Container.BindInterfacesAndSelfTo<SoundManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<BackgroundMusicPlayer>().ToSelf().FromComponentInHierarchy().AsSingle();
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<CharacterDeathPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacerRespawnPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TimerPresenter>().AsSingle().NonLazy();
        }

        private void InstallViews()
        {
            Container.Bind<DeathEffectView>().ToSelf().AsSingle();
            Container.Bind<TimerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerSpawnPoint>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RespawnColiderView>().FromComponentsInHierarchy().AsSingle();
        }

        private void InstallModels()
        {
            Container.BindInterfacesAndSelfTo<TimerModel>().AsSingle().NonLazy();
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
