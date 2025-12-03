using Enums;
using FSM.States;
using FSM.States.CharacterStates;
using System.Collections.Generic;
using Zenject;
using UnityEngine;
using UniRx;
using System;

namespace FSM
{
    public class CharacterFSM : IFixedTickable, IInitializable, IDisposable
    {
        public IObservable<CharacterState> OnStateChanged => _onStateChanged;
        public GameObject GameObject => _rigidbody.gameObject;
        public Rigidbody Rigidbody => _rigidbody;

        [Inject] private Rigidbody _rigidbody;

        [Inject] private StateMachine _stateMachine;

        [Inject] private StunnedState _stunnedState;

        private Dictionary<CharacterState, IState> _states;

        private readonly CompositeDisposable _compositeDisposable = new();

        private readonly Subject<CharacterState> _onStateChanged = new();

        private IDisposable _stunTimer;

        public void Initialize()
        {
            _states = new()
            {
                [CharacterState.Idle] = new IdleState(),
                [CharacterState.Stunned] = _stunnedState
            };

            _stateMachine.ChangeState(_states[CharacterState.Idle]);
        }

        public void ChangeToState(CharacterState state)
        {
            _stunTimer?.Dispose();

            _stateMachine.ChangeState(_states[state]);

            if (state == CharacterState.Stunned)
            {
                _stunTimer = Observable.Timer(TimeSpan.FromSeconds(4))
                    .Subscribe(_ => ChangeToState(CharacterState.Idle))
                    .AddTo(_compositeDisposable);
            }
        }

        public IState GetCurrentState()
        {
            return _stateMachine.CurrentState;
        }

        public void FixedTick() => _stateMachine.Update();

        public void Dispose() => _onStateChanged?.Dispose();
    }
}

