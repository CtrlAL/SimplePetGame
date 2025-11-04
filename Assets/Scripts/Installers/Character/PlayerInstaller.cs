using Models;
using Presenters;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Views.Scene;

namespace Services.Installers
{
    public class PlayerInstaller : CharacterInstaller
    {
        [SerializeField]
        private PlayerStatsSO _playerStatsSO;

        [SerializeField]
        private Animator _animator;

        public override void InstallBindings()
        {
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerKickPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<ThrowableInteractionModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThrowableInteractionPresenter>().AsSingle().NonLazy();
            Container.Bind<ThrowableInteractionView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IThrowableInteractor>().To<ThrowableInteractor>().AsSingle();

            Container.BindInterfacesAndSelfTo<MovePlayerPresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<CharacterAnimantionPresenter>().AsSingle();
            Container.Bind<Animator>().ToSelf().FromInstance(_animator).AsSingle();

            base.InstallBindings();
        }
    }
}