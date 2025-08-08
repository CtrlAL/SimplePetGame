using FSM;
using Models;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Assets.Scripts.Presenters
{
    public class MovePlayerPresenter : IFixedTickable, IInitializable, IDisposable
    {
        [Inject]
        private MoveCharacterModel _moveCharacterModel;

        [Inject]
        private IMover _mover;

        [Inject]
        private IPlayerInputProvider _playerInputProvider;

        [Inject]
        private PlayerStatsSO _stats;

        [Inject]
        private CharacterFSM _fsm;

        public void Initialize()
        {
            _playerInputProvider.InputActions.Enable();
            _playerInputProvider.Inputs.Jump.performed += Jump;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            _mover.Jump(this, new JumpEventArgs(_fsm, _moveCharacterModel.Rigidbody, _stats.JumpForce));
        }

        public void FixedTick()
        {
            if (_playerInputProvider.InputActions.Inputs.Move.IsPressed())
            {
                var input = _playerInputProvider.Inputs.Move.ReadValue<Vector2>();
                _mover.Move(this, new MoveEventArgs(input, _fsm, _moveCharacterModel.Rigidbody, _stats.MoveSpeed, _stats.RotationSpeed));
            }
        }

        public void Dispose()
        {
            _playerInputProvider.Inputs.Jump.performed -= Jump;
        }
    }
}
