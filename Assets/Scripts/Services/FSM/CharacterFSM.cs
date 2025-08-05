using Enums;
using FSM.States;
using FSM.States.CharacterStates;
using System.Collections.Generic;
using UnityEngine;
using Services;

namespace FSM
{
    public class CharacterFSM : MonoBehaviour
    {
        [SerializeField] GameObject _StunnedIcon;

        private StateMachine _stateMachine;
        private Dictionary<CharacterState, IState> _states;

        private void Awake()
        {
            _stateMachine = new StateMachine();

            _states = new()
            {
                [CharacterState.Idle] = new IdleState(),
                [CharacterState.Stunned] = new StunnedState(this, _StunnedIcon, new Stuner()),
            };

            _stateMachine.ChangeState(_states[CharacterState.Idle]);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        public void ChangeToState(CharacterState stateDiscriptor)
        {
            _stateMachine.ChangeState(_states[stateDiscriptor]);
        }

        public IState GetCurrentState()
        {
            return _stateMachine.CurrentState;
        }
    }
}

