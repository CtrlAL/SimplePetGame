using Models;
using ScriptableObjects;
using Services.Interfaces;
using System;
using UniRx;
using Views.UI;
using Zenject;

namespace Presenters
{
    public class EndLevelPresenter : IInitializable, IDisposable, IFixedTickable
    {
        [Inject] private IPlayerInputProvider _playerInputProvider;
        [Inject] private StatsModel _statsModel;
        [Inject] private TimerModel _timerModel;
        [Inject] private LevelSettings _levelSettings;
        [Inject] private ResultMenuView _resultMenuView;

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
                ShowResultView();
            }
        }

        private void ShowResultView()
        {
            _playerInputProvider.Inputs.Disable();
            _resultMenuView.InItScore(_statsModel.KilledCubes.Value, _timerModel.GameTime.Value);
            _resultMenuView.gameObject.SetActive(true);
        }
    }
}
