using System;
using Enums;
using Services.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace FSM.States
{
    public class StunnedState : ObservableState, IState, IDisposable
    {
        [Inject] private readonly IStuner _stunService;

        [Inject] private readonly Rigidbody _rigidbody;

        [Inject] private readonly CharacterFSM _fsm;

        private readonly CompositeDisposable _stunDisposable = new();

        public void Enter()
        {
            if (_rigidbody != null && _rigidbody.gameObject != null)
            {
                _stunService.ApplyStun(_rigidbody.gameObject, _rigidbody);

                Observable.Timer(TimeSpan.FromSeconds(4))
                    .Subscribe(_ => _fsm.ChangeToState(CharacterState.Idle))
                    .AddTo(_stunDisposable);

                _onStateEnter.OnNext(Unit.Default);
            }
        }

        public void Exit()
        {
            _stunDisposable.Clear();

            if (_rigidbody != null && _rigidbody.gameObject != null)
            {
                _stunService.RemoveStun(_rigidbody.gameObject, _rigidbody);
                _onStateExit.OnNext(Unit.Default);
            }
        }

        public void Dispose()
        {
            _stunDisposable.Dispose();
        }
    }
}