using Services.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Views.UI;
using Zenject;

namespace Presenters
{
    public class PauseMenuPresenter : IInitializable, IDisposable
    {
        [Inject] private PauseMenuView _view;

        [Inject] private IPlayerInputProvider _playerInputProvider;

        private bool _IsStarted = false;

        public void Initialize()
        {
            _view.SetNotStartedMenu();
            OpenMenu(default);
            _view.StartRestartButton.onClick.AddListener(Start);
            _view.ResumeButton.onClick.AddListener(Resume);
            _view.ExitButton.onClick.AddListener(Exit);
            _playerInputProvider.Inputs.Menu.performed += OpenMenu;
        }

        private void OpenMenu(InputAction.CallbackContext context)
        {
            _playerInputProvider.Inputs.Disable();
            _view.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        private void CloseMenu(InputAction.CallbackContext context)
        {
            _playerInputProvider.Inputs.Enable();
            _view.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Start()
        {
            if (!_IsStarted)
            {
                _view.SetStartedMenu();
                CloseMenu(default);
                _IsStarted = true;
            }
            else
            {
                Debug.Log("Reload scene");
                Scene currentScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(currentScene.name);
            }
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

        public void Dispose()
        {
            _playerInputProvider.Inputs.Menu.performed -= OpenMenu;
        }
    }
}