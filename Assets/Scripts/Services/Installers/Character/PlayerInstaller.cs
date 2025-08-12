using Models;
using Presenters;
using ScriptableObjects;
using Services.EventTriggers;
using Services.Interfaces;
using UnityEngine;
using Views.Scene;
using Views.Scene.Animation;

namespace Services.Installers
{
    [RequireComponent(typeof(CharacterAnimatior))]
    public class PlayerInstaller : CharacterInstaller
    {
        [SerializeField]
        private PlayerStatsSO _playerStatsSO;

        public override void InstallBindings()
        {
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();
            Container.Bind<PlayerRadialKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();

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