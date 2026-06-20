using System;
using UniRx;

namespace Models
{
    public class ImpactHandlerModel : IDisposable
    {
        public ReactiveProperty<int> CurrentWeakHitCount = new(0);

        public Subject<Unit> OnStrongHit { get; } = new();
        public Subject<Unit> OnWeakHit { get; } = new();
        public Subject<Unit> OnThresholdReached { get; } = new();

        public void Dispose()
        {
            CurrentWeakHitCount?.Dispose();
            OnStrongHit?.Dispose();
            OnWeakHit?.Dispose();
            OnThresholdReached?.Dispose();
        }
    }
}
