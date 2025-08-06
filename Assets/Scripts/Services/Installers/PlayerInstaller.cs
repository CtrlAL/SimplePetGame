using Assets.Scripts.Services.Interfaces;
using ScriptableObjects;
using Services.EventTriggers;
using UnityEngine;

namespace Services.Installers
{
    public class PlayerInstaller : CharacterInstaller
    {
        [SerializeField]
        private PlayerStatsSO _playerStatsSO;

        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<IPlayerMovementInputHandler>().To<PlayerMovementInputHandler>().AsSingle().NonLazy();
            Container.Bind<AbstractStatsSO>().ToSelf().FromInstance(_playerStatsSO).AsSingle();
            Container.Bind<PlayerRadialKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();
        }
    }
}
