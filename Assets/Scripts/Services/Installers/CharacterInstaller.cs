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
            Container.Bind<CharacterFSM>().ToSelf().AsSingle();
            Container.Bind<Rigidbody>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ImpactHandlerView>().FromComponentInHierarchy().AsSingle();

            Container.Bind<IFatigue>().To<Fatigue>().AsSingle();
            Container.Bind<FatigueModel>().ToSelf().AsSingle();
            Container.Bind<FatiguePresenter>().ToSelf().AsSingle().NonLazy();

            Container.Bind<ImpactHandlerModel>().ToSelf().AsSingle().NonLazy();
            Container.Bind<ImpactHandlerPresenter>().ToSelf().AsSingle().NonLazy();
        }
    }
}