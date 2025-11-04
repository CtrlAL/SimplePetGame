using Models;
using FSM;
using Presenters;
using Zenject;
using UnityEngine;
using Services.Interfaces;
using FSM.States;
using Views;

namespace Services.Installers
{
    public abstract class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromComponentOnRoot().AsCached();
            Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();

            Container.BindInterfacesAndSelfTo<CharacterFSM>().AsSingle();
            Container.Bind<StateMachine>().ToSelf().AsSingle();


            Container.Bind<IMover>().To<Mover>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveCharacterModel>().AsSingle();

            Container.Bind<IKiker>().To<Kicker>().AsSingle();
            Container.Bind<ImpactDetectorView>().FromComponentOnRoot().AsSingle();
            Container.BindInterfacesAndSelfTo<ImpactHandlerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ImpactHandlerPresenter>().AsSingle().NonLazy();

            Container.Bind<IFatigueManager>().To<FatigueManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<FatigueModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<FatigueRestorationService>().AsSingle().NonLazy();            


            Container.Bind<IStuner>().To<Stuner>().AsSingle();
            Container.Bind<StunnedState>().ToSelf().AsSingle();
            Container.Bind<IStunEffectService>().To<StunEffectService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CharacterStunPresenter>().AsSingle();
        }
    }
}