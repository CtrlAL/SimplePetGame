using System;
using UniRx;

namespace Models
{
    public class EnemyKickModel : IDisposable
    {
        private readonly Subject<Unit> _onKickPerformed = new();

        public IObservable<Unit> OnKickPerformed => _onKickPerformed;

        public void RaiseKickPerformed() => _onKickPerformed.OnNext(Unit.Default);

        public void Dispose()
        {
            _onKickPerformed?.Dispose();
        }
    }
}
