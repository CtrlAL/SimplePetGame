using Services;
using Services.Interfaces;
using Unity.VisualScripting;
using Zenject;

public partial class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallPlayerInputs();
        InstallServices();
        InstallViews();
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

    private void InstallViews()
    {
        Container.Bind<DeathEffectPresenter>().ToSelf().AsSingle();
        Container.Bind<StunEffectPresenter>().ToSelf().AsSingle();
    }
}