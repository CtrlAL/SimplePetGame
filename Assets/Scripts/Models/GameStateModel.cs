using System;
using UniRx;

namespace Models
{
    public class GameStateModel : IDisposable
    {
        public ReactiveProperty<bool> IsStarted = new(false);

        public void Dispose()
        {
            IsStarted?.Dispose();
        }
    }
}