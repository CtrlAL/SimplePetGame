using FSM.States.CharacterStates;
using FSM;
using Helpers;
using Services.Interfaces;
using System;
using UniRx;
using UnityEngine.InputSystem;
using UnityEngine;
using Zenject;
using Models;
using System.Linq;
using Enums;
using Services.Sound;
using Views;

namespace Presenters
{
    public class ThrowableInteractionPresenter : IInitializable, IDisposable, IFixedTickable
    {
        private readonly ThrowableInteractionModel _model;
        private readonly ThrowableInteractionView _view;
        private readonly IThrowableInteractor _interactor;
        private readonly IPlayerInputProvider _inputProvider;
        private readonly CharacterFSM _fsm;
        private readonly SoundManager _soundManager;

        private CompositeDisposable _disposebles;

        public ThrowableInteractionPresenter(
            ThrowableInteractionModel model,
            ThrowableInteractionView view,
            IThrowableInteractor interactor,
            IPlayerInputProvider inputProvider,
            CharacterFSM fsm,
            SoundManager soundManager)
        {
            _interactor = interactor;
            _inputProvider = inputProvider;
            _fsm = fsm;
            _view = view;
            _model = model;
            _soundManager = soundManager;
            _disposebles = new CompositeDisposable();
        }

        public void Initialize()
        {
            _view.OnObjectEnteredRange.Subscribe(go => OnEnterRange(go))
                .AddTo(_disposebles);
            _view.OnObjectExitedRange.Subscribe(go => OnExitRange(go)).
                AddTo(_disposebles);
            _view.OnObjectStayedInRange.Subscribe(go => OnStayInRange(go))
                .AddTo(_disposebles);

            _inputProvider.Inputs.Pickup.performed += OnPickup;
            _inputProvider.Inputs.Throw.performed += OnThrow;
        }

        public void Dispose()
        {
            _inputProvider.Inputs.Pickup.performed -= OnPickup;
            _inputProvider.Inputs.Throw.performed -= OnThrow;

            _disposebles.Dispose();
        }

        private void OnPickup(InputAction.CallbackContext context)
        {
            if (!(_fsm.GetCurrentState() is IdleState)) return;

            if (_model.IsHolding)
            {
                _interactor.Put(_view.transform);
            }
            else
            {
                PickupClosest();
            }
        }

        private void OnThrow(InputAction.CallbackContext context)
        {
            if (_fsm.GetCurrentState() is IdleState && _model.IsHolding)
            {
                _interactor.Throw(_view.transform);
                _soundManager.PlaySound(1, SoundType.Throw);
            }
        }

        private void PickupClosest()
        {
            if (_model.AllowedThrowables.Count == 0) return;

            var closest = _model.AllowedThrowables
                .Where(go => go != null && GameHelpers.IsGrounded(go))
                .OrderBy(go => Vector3.Distance(_view.transform.position, go.transform.position))
                .FirstOrDefault();

            if (closest != null)
            {
                _interactor.Pickup(closest, _view.ThrowablesSlot.transform);
            }
        }

        public void OnEnterRange(GameObject obj) => AddIfValid(obj);
        public void OnStayInRange(GameObject obj) => AddIfValid(obj);
        public void OnExitRange(GameObject obj) => _model.AllowedThrowables.Remove(obj);
        public void CleanupNulls() => _model.AllowedThrowables.RemoveWhere(item => item == null);

        private void AddIfValid(GameObject obj)
        {
            if (GameHelpers.IsThrowable(obj) && GameHelpers.IsGrounded(obj))
                _model.AllowedThrowables.Add(obj);
        }

        public void FixedTick()
        {
            CleanupNulls();
        }
    }
}
