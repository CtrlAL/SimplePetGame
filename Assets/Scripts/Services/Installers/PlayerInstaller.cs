using Models;
using Presenters;
using ScriptableObjects;
using Services.EventTriggers;
using Services.Interfaces;
using UnityEngine;
using Views.Scene;
using Zenject;

namespace Services.Installers
{
    public class PlayerInstaller : CharacterInstaller
    {
        [SerializeField]
        private PlayerStatsSO _playerStatsSO;

        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<IPlayerMovementInputHandler>().To<PlayerMovementInputHandler>().AsSingle();
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();
            Container.Bind<PlayerRadialKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();

            Container.Bind<IFixedTickable>().To<PlayerUpdate>().AsSingle();

            Container.BindInterfacesAndSelfTo<ThrowableInteractionModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ThrowableInteractionPresenter>().AsSingle().NonLazy();
            Container.Bind<ThrowableInteractionView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IThrowableInteractor>().To<ThrowableInteractor>().AsSingle();
        }
    }
}
