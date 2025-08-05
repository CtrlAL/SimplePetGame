using System;
using UniRx;

namespace Models
{
    public class ImpactHandlerModel : IDisposable
    {
        public ReactiveProperty<int> CurrentWeakHitCount = new(0);
        public void Dispose()
        {
            CurrentWeakHitCount?.Dispose();
        }
    }
}

