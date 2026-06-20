using System;
using UniRx;

namespace Models
{
    public class StatsModel : IDisposable
    {
        private readonly ReactiveProperty<int> _killedCubes = new(0);

        public IReactiveProperty<int> KilledCubes => _killedCubes;

        public void Dispose()
        {
            _killedCubes?.Dispose();
        }
    }
}
