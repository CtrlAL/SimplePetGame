using System;
using UniRx;

namespace Models
{
    public class StatsModel : IDisposable
    {
        public ReactiveProperty<float> KilledCubes = new(0);

        public void Dispose()
        {
            KilledCubes?.Dispose();
        }
    }
}
