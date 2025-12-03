using System;
using UniRx;

namespace Models
{
    public class StatsModel : IDisposable
    {
        public ReactiveProperty<int> KilledCubes = new(0);

        public void Dispose()
        {
            KilledCubes?.Dispose();
        }
    }
}
