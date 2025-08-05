using ScriptableObjects;
using Services.EventTriggers;

namespace Services.Installers
{
    public class EnemyInstaller : CharacterInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<AbstractStatsSO>().To<EnemyStatsSO>().AsSingle();
            Container.Bind<EnemyDelayedKickTrigger>().ToSelf().FromComponentInHierarchy().AsSingle();
        }
    }
}
