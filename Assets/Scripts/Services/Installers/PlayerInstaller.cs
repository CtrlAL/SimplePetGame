using ScriptableObjects;
using Services.EventTriggers;

namespace Services.Installers
{
    public class PlayerInstaller : CharacterInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<AbstractStatsSO>().To<PlayerStatsSO>().AsSingle();
            Container.Bind<PlayerRadialKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();
        }
    }
}
