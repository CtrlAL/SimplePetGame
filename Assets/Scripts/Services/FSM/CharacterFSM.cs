using Enums;
using FSM.States;
using FSM.States.CharacterStates;
using System.Collections.Generic;
using Services;
using Zenject;
using UnityEngine;
using UniRx;

namespace FSM
{
    public class CharacterFSM : ITickable, IInitializable
    {
        [Inject]
        private readonly Rigidbody _rigidbody;

        private StateMachine _stateMachine;

        private Dictionary<CharacterState, IState> _states;

        private readonly CompositeDisposable _disposables = new();

        public GameObject GameObject => _rigidbody.gameObject;
        public Rigidbody Rigidbody => _rigidbody;

        public void Initialize()
        {
            _stateMachine = new StateMachine();

            var state = new StunnedState(new Stuner(), _rigidbody.gameObject, _rigidbody);

            state.OnStunEnd
                .Subscribe(_ => ChangeToState(CharacterState.Idle))
                .AddTo(_disposables);

            _states = new()
            {
                [CharacterState.Idle] = new IdleState(),
                [CharacterState.Stunned] = state
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

        public void Tick()
        {
            _stateMachine.Update();
            _disposables?.Dispose();
        }
    }
}

