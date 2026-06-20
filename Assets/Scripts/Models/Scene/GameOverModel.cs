using System;
using UniRx;

namespace Models
{
    public class GameOverModel : IDisposable
    {
        public Subject<Unit> GameOver = new();
        public void Dispose()
        {
            GameOver?.Dispose();
        }
    }
}

