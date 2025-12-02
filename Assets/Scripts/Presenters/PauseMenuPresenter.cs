using Services.Interfaces;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Presenters
{
    public class PauseMenuPresenter : IInitializable
    {
        [Inject] private PauseMenu _view;

        [Inject] private IPlayerInputProvider _playerInputProvider;

        private CompositeDisposable _disposables = new();

        public void Initialize()
        {
            _view.SetNotStartedMenu();
            _view.StartRestartButton.onClick.AddListener(Start);
            _view.ResumeButton.onClick.AddListener(Resume);
            _view.ExitButton.onClick.AddListener(Exit);

            _playerInputProvider.Inputs.Menu.performed += OpenMenu;
        }

        private void OpenMenu(InputAction.CallbackContext context)
        {
            _view.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        private void CloseMenu(InputAction.CallbackContext context)
        {
            _view.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Start()
        {
            _view.SetStartedMenu();
            CloseMenu(default);
        }

        public void Resume()
        {
            CloseMenu(default);
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