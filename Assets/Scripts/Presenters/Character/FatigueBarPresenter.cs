using Models;
using ScriptableObjects;
using System;
using UniRx;
using Views.UI;
using Zenject;

namespace Presenters
{
    public class FatigueBarPresenter : IInitializable, IDisposable
    {
        [Inject] private FatigueModel _model;
        [Inject] private PlayerStats _playerStats;
        [Inject] private FatigueBarView _view;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _model.CurrentFatigue
                .Subscribe(SetPercent);
        }

        public void SetPercent(float currentFutigue)
        {
            _view.SetValue((_playerStats.Fatigue - currentFutigue) / _playerStats.Fatigue);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }
    }
}