using System;
using UniRx;
public class ImpactHandlerModel : IDisposable
{
    public ReactiveProperty<int> CurrentWeakHitCount = new(0);
    public void Dispose()
    {
        CurrentWeakHitCount?.Dispose();
    }
}
