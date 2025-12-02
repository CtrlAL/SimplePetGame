using Cysharp.Threading.Tasks;
using Enums;
using ScriptableObjects;
using Services.Interfaces;
using Services.Sound;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Presenters
{
    public class MovePlayerPresenter : IFixedTickable, IInitializable, IDisposable
    {
        [Inject] private IMover _mover;

        [Inject] private IPlayerInputProvider _playerInputProvider;

        [Inject] private PlayerStats _stats;

        [Inject] private SoundManager _soundManager;

        private CompositeDisposable _disposables = new();

        public void Initialize()
        {
            _playerInputProvider.Inputs.Jump.performed += Jump;

            _mover.OnJumped
                .Subscribe(_ => _soundManager.PlaySound(1, SoundType.Jump))
                .AddTo(_disposables);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            _mover.Jump(_stats.JumpForce);
        }

        public void FixedTick()
        {
            if (_playerInputProvider.InputActions.Inputs.Move.IsPressed())
            {
                var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
                _mover.Move(input, _stats.MoveSpeed, _stats.RotationSpeed);
            }
        }

        public void Dispose()
        {
            _playerInputProvider.InputActions.Disable();
            _playerInputProvider.Inputs.Jump.performed -= Jump;

            _disposables?.Dispose();
        }
    }
}
