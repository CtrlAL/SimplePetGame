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

        private readonly CompositeDisposable _disposables = new();

        private readonly Subject<CharacterState> _onStateChanged = new();

        public void Initialize()
        {
            _stunnedState.OnStateExit
                .Subscribe(_ => ChangeToState(CharacterState.Idle))
                .AddTo(_disposables);

            _states = new()
            {
                [CharacterState.Idle] = new IdleState(),
                [CharacterState.Stunned] = _stunnedState
            };

            _stateMachine.ChangeState(_states[CharacterState.Idle]);
        }

        public void ChangeToState(CharacterState stateDiscriptor)
        {
            _stateMachine.ChangeState(_states[stateDiscriptor]);
            _onStateChanged.OnNext(stateDiscriptor);
        }

        public IState GetCurrentState()
        {
            return _stateMachine.CurrentState;
        }

        public void FixedTick() => _stateMachine.Update();

        public void Dispose() => _onStateChanged?.Dispose();
    }
}

