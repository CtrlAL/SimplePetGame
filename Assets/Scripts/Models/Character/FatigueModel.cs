using System;
using UniRx;

namespace Models
{
    public class FatigueModel : IDisposable
    {
        public ReactiveProperty<float> CurrentFatigue = new(0);

        public void Dispose()
        {
            CurrentFatigue?.Dispose();
        }
    }
}
