using Presenters;
using Zenject;

public class VoidZoneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<VoidZoneView>().FromComponentOnRoot().AsSingle();
        Container.BindInterfacesAndSelfTo<VoidZoneKillPresenter>().AsSingle();
    }
}
