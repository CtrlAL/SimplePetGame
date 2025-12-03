using ScriptableObjects;
using System;
using UniRx;
using Zenject;

namespace Presenters
{
    public class EndLevelPresenter : IInitializable, IDisposable, IFixedTickable
    {
        [Inject] private TimerModel _timerModel;

        [Inject] private LevelSettings _levelSettings;

        private CompositeDisposable _compositeDisposable;
        public void Initialize()
        {
            
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }

        public void FixedTick()
        {
            if (_timerModel.GameTime.Value >= _levelSettings.LevelDuration)
            {
                //Something hepends
            }
        }
    }
}
