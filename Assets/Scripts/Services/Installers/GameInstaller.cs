using ScriptableObjects;
using Services;
using Services.Interfaces;
using Views.Scene.Characters;
using Zenject;

public partial class GameInstaller : MonoInstaller
{
    private PlayerStatsSO _playerStatsSO;
    private EnemyStatsSO _enemyStatsSO;
    private KickImpactSettigns _kickImpactSettigns;
    private CharacterVFX _characterVFX;
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
        Container.Bind<PlayerMovementInputHandler>().ToSelf().AsSingle();
    }


    private void InstallModels()
    {
        Container.Bind<ImpactHandlerModel>().ToSelf().AsSingle().NonLazy();
    }

    private void InstallPresenters()
    {
    }

    private void InstallViews()
    {
        Container.Bind<DeathEffectPresenter>().ToSelf().AsSingle();
        Container.Bind<StunEffectPresenter>().ToSelf().AsSingle();
        Container.Bind<ImpactHandler>().ToSelf().AsSingle();
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