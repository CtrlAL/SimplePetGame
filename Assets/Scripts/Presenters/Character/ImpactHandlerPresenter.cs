using Constants;
using Enums;
using FSM;
using FSM.States.CharacterStates;
using Models;
using Services.Interfaces;
using System;
using UniRx;
using Views;
using Zenject;

namespace Presenters
{
    public class ImpactHandlerPresenter : IInitializable, IDisposable
    {
        [Inject] private KickImpactSettings _settings;
        [Inject] private ImpactDetectorView _impactHandler;
        [Inject] private ImpactHandlerModel _model;
        [Inject] private CharacterFSM _characterFSM;
        [Inject] private IFatigueManager _fatigueService;

        private readonly CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _impactHandler.OnImpactDetected
                .Where(_ => _characterFSM.GetCurrentState() is IdleState)
                .Subscribe(HandleImpact)
                .AddTo(_compositeDisposable);

            _model.CurrentWeakHitCount
                .Where(count => count >= _settings.WeakHitCountNeeded)
                .Subscribe(_ => _model.RaiseThresholdReached())
                .AddTo(_compositeDisposable);
        }

        private void HandleImpact(float impactForce)
        {
            if (impactForce > _settings.MinStrongImpact)
            {
                _model.RaiseStrongHit();
                ApplyStrongHit();
            }
            else if (impactForce > _settings.MinWeakImpact)
            {
                _model.RaiseWeakHit();
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
            _fatigueService.MakeFatigueDamage(FatigueDamage.WeakHitDamage);
            _model.CurrentWeakHitCount.Value++;
        }

        private void EnterStunnedState()
        {
            _characterFSM.ChangeToState(CharacterState.Stunned);
        }

        public void Dispose()
        {
            _compositeDisposable?.Dispose();
        }
    }
}
