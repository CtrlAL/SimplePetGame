using System;
using UniRx;

namespace Models
{
    public class FatigueModel : IDisposable
    {
        private readonly ReactiveProperty<float> _currentFatigue = new(0);

        public IReactiveProperty<float> CurrentFatigue => _currentFatigue;

        public void Dispose()
        {
            _currentFatigue?.Dispose();
        }
    }
}
