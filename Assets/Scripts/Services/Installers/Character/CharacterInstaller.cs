using Models;
using FSM;
using Presenters;
using Zenject;
using Views.Scene.Characters;
using UnityEngine;
using Services.Interfaces;

namespace Services.Installers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Transform))]
    public abstract class CharacterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Transform>().FromComponentOnRoot().AsSingle();
            Container.Bind<Rigidbody>().FromComponentOnRoot().AsSingle();

            Container.BindInterfacesAndSelfTo<CharacterFSM>().AsSingle();
            
            Container.Bind<IFatigue>().To<Fatigue>().AsSingle();
            Container.BindInterfacesAndSelfTo<FatigueModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<FatiguePresenter>().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<ImpactHandlerModel>().AsSingle();
            Container.BindInterfacesAndSelfTo<ImpactHandlerPresenter>().AsSingle().NonLazy();
            Container.Bind<ImpactHandlerView>().FromComponentInHierarchy().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MoveCharacterModel>().AsSingle();
        }
    }
}