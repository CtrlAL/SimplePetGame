using ScriptableObjects;
using Services.EventTriggers;
using Services.Interfaces;
using UnityEngine;
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
        }
    }
}
