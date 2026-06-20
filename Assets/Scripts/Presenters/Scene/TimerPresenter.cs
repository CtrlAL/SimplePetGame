using ScriptableObjects;
using System;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class TimerPresenter : IInitializable, IDisposable, IFixedTickable
    {
        [Inject] private LevelSettings _levelSettings;
        [Inject] private TimerModel _timerModel;
        [Inject] private TimerView _timerView;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = _timerModel.GameTime
                .Subscribe(OnTimeUpdated);
        }

        public void Dispose()
        {
            _subscription?.Dispose();
        }

        private void OnTimeUpdated(float totalSeconds)
        {
            totalSeconds = Mathf.Max(0f, totalSeconds);

            int minutes = (int)(totalSeconds / 60f);
            int seconds = (int)(totalSeconds % 60f);

            string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);

            _timerView.TimerText.text = formattedTime;
        }

        public void FixedTick()
        {
            _timerModel.GameTime.Value += Time.deltaTime;
        }
    }
}