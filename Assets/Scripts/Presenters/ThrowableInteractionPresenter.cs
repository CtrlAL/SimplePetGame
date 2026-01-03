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
using Extensions;

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

        private CompositeDisposable _compositeDisposable;

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
            _compositeDisposable = new CompositeDisposable();
        }

        public void Initialize()
        {
            _view.OnObjectEnteredRange.Subscribe(go => OnEnterRange(go))
                .AddTo(_compositeDisposable);
            _view.OnObjectExitedRange.Subscribe(go => OnExitRange(go)).
                AddTo(_compositeDisposable);
            _view.OnObjectStayedInRange.Subscribe(go => OnStayInRange(go))
                .AddTo(_compositeDisposable);

            _inputProvider.Inputs.Pickup.performed += OnPickup;
            _inputProvider.Inputs.Throw.performed += OnThrow;
        }

        public void FixedTick()
        {
            if (_model.IsHolding && !_model.PickedObject.Value.IsThrowable())
            {
                _interactor.Put(_view.transform);
            }

            CleanupNulls();
        }

        public void Dispose()
        {
            _inputProvider.Inputs.Pickup.performed -= OnPickup;
            _inputProvider.Inputs.Throw.performed -= OnThrow;

            _compositeDisposable.Dispose();
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

        private void OnEnterRange(GameObject obj) => AddIfValid(obj);
        private void OnStayInRange(GameObject obj) => AddIfValid(obj);
        private void OnExitRange(GameObject obj) => _model.AllowedThrowables.Remove(obj);
        private void CleanupNulls() => _model.AllowedThrowables.RemoveWhere(item => item == null);

        private void AddIfValid(GameObject obj)
        {
            if (obj.IsThrowable() && GameHelpers.IsGrounded(obj))
                _model.AllowedThrowables.Add(obj);
        }
    }
}
