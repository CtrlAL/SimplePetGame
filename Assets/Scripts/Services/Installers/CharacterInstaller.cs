using Models;
using FSM;
using Presenters;
using Zenject;
using Views.Scene.Characters;
using UnityEngine;
using Services.Interfaces;

namespace Services.Installers
{
    public abstract class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CharacterFSM>().AsSingle();
            Container.Bind<Rigidbody>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IFatigue>().To<Fatigue>().AsSingle();
            Container.BindInterfacesAndSelfTo<FatigueModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<FatiguePresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<ImpactHandlerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ImpactHandlerPresenter>().AsSingle().NonLazy();
            Container.Bind<ImpactHandlerView>().FromComponentInHierarchy().AsSingle();
        }
    }
}