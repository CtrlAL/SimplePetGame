using Presenters;
using Services.Interfaces;
using Services.Sound;
using Views;
using Views.UI;
using Zenject;

namespace Services.Installers
{
    public partial class GameSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallPlayerInputs();
            InstallServices();
            InstallPresenters();
            InstallUIPresenters();
            InstallViews();
            InstallUIViews();
            InstallModels();
            InstallSound();
        }

        private void InstallServices()
        {
            Container.Bind<IDeathEffectService>().To<DeathEffectService>().AsSingle();
            Container.Bind<IEnemyPool>().To<EnemyPool>().AsSingle();
            Container.Bind<IDeathEffectPool>().To<DeathEffectPool>().AsSingle();
            Container.Bind<IStunEffectPool>().To<StunEffectPool>().AsSingle();
            Container.BindInterfacesTo<EnemyFactory>().AsSingle().NonLazy();
        }

        private void InstallPlayerInputs()
        {
            Container.Bind<IPlayerInputProvider>().To<PlayerInputProvider>().AsSingle().NonLazy();
            Container.Bind<PlayerInputActions>().ToSelf().AsSingle();
        }

        private void InstallSound()
        {
            Container.BindInterfacesAndSelfTo<SoundManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<BackgroundMusicPlayer>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallPresenters()
        {
            Container.BindInterfacesAndSelfTo<CharacterDeathPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<CharacerRespawnPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<TimerPresenter>().AsSingle().NonLazy();
        }

        private void InstallViews()
        {
            Container.Bind<TimerView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PlayerSpawnPointView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<RespawnColiderView>().FromComponentsInHierarchy().AsSingle();
        }

        private void InstallUIViews()
        {
            Container.Bind<FatigueBarView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<PauseMenu>().FromComponentInHierarchy().AsSingle();
        }

        private void InstallUIPresenters()
        {
            Container.BindInterfacesAndSelfTo<PauseMenuPresenter>().AsSingle().NonLazy();
        }

        private void InstallModels()
        {
            Container.BindInterfacesAndSelfTo<TimerModel>().AsSingle().NonLazy();
        }
    }
}
