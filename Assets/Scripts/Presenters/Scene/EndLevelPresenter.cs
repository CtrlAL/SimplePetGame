using Models;
using ScriptableObjects;
using Services.Interfaces;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Views.UI;
using Zenject;

namespace Presenters
{
    public class EndLevelPresenter : IInitializable, IFixedTickable, IDisposable
    {
        [Inject] private GameOverModel _gameOverModel;
        [Inject] private IPlayerInputProvider _playerInputProvider;
        [Inject] private StatsModel _statsModel;
        [Inject] private TimerModel _timerModel;
        [Inject] private LevelSettings _levelSettings;
        [Inject] private ResultMenuView _resultMenuView;

        private readonly CompositeDisposable _compositeDisposable = new();
        private bool _resultShown;

        public void Initialize()
        {
            _resultMenuView.RestartButton.onClick.AddListener(Restart);
            _resultMenuView.ExitButton.onClick.AddListener(Exit);
            _resultMenuView.BackButton.onClick.AddListener(_resultMenuView.HideScore);
            _resultMenuView.ScoreButton.onClick.AddListener(_resultMenuView.ShowScore);

            _gameOverModel.GameOver
                .Subscribe(_ => ShowResultView())
                .AddTo(_compositeDisposable);
        }

        public void FixedTick()
        {
            if (_resultShown) return;

            float minutes = (float)(_timerModel.GameTime.Value / 60f);

            if (minutes >= _levelSettings.LevelDuration)
            {
                ShowResultView();
            }
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
            Time.timeScale = 1;
            _resultMenuView.RestartButton.onClick.RemoveListener(Restart);
            _resultMenuView.ExitButton.onClick.RemoveListener(Exit);
            _resultMenuView.BackButton.onClick.RemoveListener(_resultMenuView.HideScore);
            _resultMenuView.ScoreButton.onClick.RemoveListener(_resultMenuView.ShowScore);
        }

        private void ShowResultView()
        {
            if (_resultShown) return;
            _resultShown = true;

            Time.timeScale = 0;
            _playerInputProvider.Inputs.Disable();
            _resultMenuView.InItScore(_statsModel.KilledCubes.Value, _timerModel.GameTime.Value);
            _resultMenuView.gameObject.SetActive(true);
        }

        private void Restart()
        {
            Time.timeScale = 1;
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
