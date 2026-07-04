using Models;
using Presenters;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Views;

namespace Services.Installers
{
    public class PlayerInstaller : CharacterInstaller
    {
        [SerializeField] private Animator _animator;

        public override void InstallBindings()
        {
            Container.Bind<IMover>().To<PlayerMover>().AsSingle();

            Container.Bind<AbstractStats>().To<PlayerStats>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerKickPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<ThrowableInteractionModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThrowableInteractionPresenter>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<OutlinePresenter>().AsSingle().NonLazy();
            Container.Bind<ThrowableInteractionView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IThrowableInteractor>().To<ThrowableInteractor>().AsSingle();

            Container.BindInterfacesAndSelfTo<MovePlayerPresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CharacterAnimationPresenter>().AsSingle();
            Container.Bind<Animator>().ToSelf().FromInstance(_animator).AsSingle();

            Container.BindInterfacesAndSelfTo<FatigueBarPresenter>().AsSingle();

            base.InstallBindings();
        }
    }
}