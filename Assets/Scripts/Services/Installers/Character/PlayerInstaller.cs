using Models;
using Presenters;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Views.Scene;
using Views.Scene.Animation;

namespace Services.Installers
{
    public class PlayerInstaller : CharacterInstaller
    {
        [SerializeField]
        private PlayerStatsSO _playerStatsSO;

        public override void InstallBindings()
        {
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerKickPresenter>().AsSingle();

            Container.BindInterfacesAndSelfTo<ThrowableInteractionModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThrowableInteractionPresenter>().AsSingle().NonLazy();
            Container.Bind<ThrowableInteractionView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IThrowableInteractor>().To<ThrowableInteractor>().AsSingle();

            Container.BindInterfacesAndSelfTo<MovePlayerPresenter>().AsSingle().NonLazy();

            Container.Bind<CharacterAnimatior>().FromComponentInHierarchy().AsSingle();

            base.InstallBindings();
        }
    }
}