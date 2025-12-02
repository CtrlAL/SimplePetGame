using ScriptableObjects;
using System;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class TimerPresenter : IFixedTickable, IInitializable, IDisposable
    {
        [Inject] public LevelSettings LevelSettings;

        [Inject] public TimerModel TimerModel;

        [Inject] public TimerView TimerView;

        private IDisposable _subscription;

        public void Initialize()
        {
            _subscription = TimerModel.GameTime
                .Subscribe(OnTimeUpdated);
        }

        public void FixedTick()
        {
            while (TimerModel.GameTime.Value / 60 < LevelSettings.LevelDuration)
            {
                TimerModel.GameTime.Value += Time.fixedDeltaTime;
            }
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

            TimerView.TimerText.text = formattedTime;
        }
    }
}