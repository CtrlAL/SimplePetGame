using System;
using UniRx;

namespace Models
{
    public class ImpactHandlerModel : IDisposable
    {
        public ReactiveProperty<int> CurrentWeakHitCount = new(0);

        private readonly Subject<Unit> _onStrongHit = new();
        private readonly Subject<Unit> _onWeakHit = new();
        private readonly Subject<Unit> _onThresholdReached = new();

        public IObservable<Unit> OnStrongHit => _onStrongHit;
        public IObservable<Unit> OnWeakHit => _onWeakHit;
        public IObservable<Unit> OnThresholdReached => _onThresholdReached;

        public void RaiseStrongHit() => _onStrongHit.OnNext(Unit.Default);
        public void RaiseWeakHit() => _onWeakHit.OnNext(Unit.Default);
        public void RaiseThresholdReached() => _onThresholdReached.OnNext(Unit.Default);

        public void Dispose()
        {
            CurrentWeakHitCount?.Dispose();
            _onStrongHit?.Dispose();
            _onWeakHit?.Dispose();
            _onThresholdReached?.Dispose();
        }
    }
}
