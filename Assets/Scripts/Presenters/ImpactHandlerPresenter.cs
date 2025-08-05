using FSM;
using System;
using UniRx;
using Zenject;
using Enums;
using FSM.States.CharacterStates;
using Views.Scene.Characters;
using Models;
using Assets.Scripts.Services.Interfaces;
using Constants;

namespace Presenters
{
    public class ImpactHandlerPresenter : IInitializable, IDisposable
    {
        private readonly Subject<Unit> _onStrongHit = new();
        private readonly Subject<Unit> _onWeakHit = new();
        private readonly Subject<Unit> _onThresholdReached = new();

        public IObservable<Unit> OnStrongHit => _onStrongHit;
        public IObservable<Unit> OnWeakHit => _onWeakHit;
        public IObservable<Unit> OnThresholdReached => _onThresholdReached;

        [Inject] private KickImpactSettigns _settings;
        [Inject] private ImpactHandlerView _impactHandler;
        [Inject] private ImpactHandlerModel _model;
        [Inject] private CharacterFSM _characterFSM;
        [Inject] private IFatigue _fatigueService;

        private readonly CompositeDisposable _disposables = new();

        public void Initialize()
        {
            _impactHandler.OnImpactDetected
                .Where(_ => _characterFSM.GetCurrentState() is IdleState)
                .Subscribe(HandleImpact)
                .AddTo(_disposables);

            _model.CurrentWeakHitCount
                .Where(count => count >= _settings.WeakHitCountNeeded)
                .Subscribe(_ => _onThresholdReached.OnNext(Unit.Default))
                .AddTo(_disposables);
        }

        private void HandleImpact(float impactForce)
        {
            if (impactForce > _settings.MinStrongImpact)
            {
                _onStrongHit.OnNext(Unit.Default);
                ApplyStrongHit();
            }
            else if (impactForce > _settings.MinWeakImpact)
            {
                _onWeakHit.OnNext(Unit.Default);
                ApplyWeakHit();
            }
        }

        private void ApplyStrongHit()
        {
            _model.CurrentWeakHitCount.Value = 0;
            _fatigueService.MakeFatigueDamage(FatigueDamage.StrongHitDamage);
            EnterStunnedState();
        }

        private void ApplyWeakHit()
        {
            _model.CurrentWeakHitCount.Value++;
        }

        private void EnterStunnedState()
        {
            _characterFSM.ChangeToState(CharacterState.Stunned);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _onStrongHit?.Dispose();
            _onWeakHit?.Dispose();
            _onThresholdReached?.Dispose();
        }
    }
}