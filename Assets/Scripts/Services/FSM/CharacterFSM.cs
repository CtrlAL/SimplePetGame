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
        [Inject] private Rigidbody _rigidbody;

        [Inject] private StateMachine _stateMachine;

        [Inject] private StunnedState _stunnedState;

        private Dictionary<CharacterState, IState> _states;

        private readonly CompositeDisposable _disposables = new();

        public GameObject GameObject => _rigidbody.gameObject;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize()
        {
            _stunnedState.OnStunEnd
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
        }

        public IState GetCurrentState()
        {
            return _stateMachine.CurrentState;
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        public void FixedTick()
        {
            _stateMachine.Update();
        }
    }
}

