using Models;
using Services.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace Presenters
{
    public class PauseMenuPresenter : IInitializable
    {
        [Inject] private PauseMenu _view;

        [Inject] private GameStateModel _gameStateModel;

        [Inject] private IPlayerInputProvider _playerInputProvider;

        private CompositeDisposable _disposables = new();

        public void Initialize()
        {
            _view.SetNotStartedMenu();
            _view.StartRestartButton.onClick.AddListener(Start);
            _view.ResumeButton.onClick.AddListener(Resume);
            _view.ExitButton.onClick.AddListener(Exit);
        }

        public void Start()
        {
            _view.SetStartedMenu();
            Time.timeScale = 1;
        }

        public void Resume()
        {
            Time.timeScale = 1;
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
