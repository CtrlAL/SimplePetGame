using System;
using System.Collections.Generic;
using UniRx;
using Zenject;
using Enums;
using FSM.States;
using FSM.States.CharacterStates;

namespace FSM
{
    public class CharacterFSM : IFixedTickable, IInitializable, IDisposable
    {
        public IObservable<CharacterState> OnStateChanged => _onStateChanged;

        [Inject] private StateMachine _stateMachine;

        [Inject] private StunnedState _stunnedState;

        private Dictionary<CharacterState, IState> _states;

        private readonly Subject<CharacterState> _onStateChanged = new();

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
            _stateMachine.ChangeState(_states[state]);
        }

        public IState GetCurrentState() => _stateMachine.CurrentState;

        public void FixedTick() => _stateMachine.Update();

        public void Dispose()
        {
            _stunnedState.Dispose();
            _onStateChanged?.Dispose();
        }
    }
}

