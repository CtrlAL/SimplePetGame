using System;
using UniRx;

namespace Models
{
    public class GameOverModel : IDisposable
    {
        private readonly Subject<Unit> _gameOver = new();

        public IObservable<Unit> GameOver => _gameOver;

        public void RaiseGameOver() => _gameOver.OnNext(Unit.Default);

        public void Dispose()
        {
            _gameOver?.Dispose();
        }
    }
}
