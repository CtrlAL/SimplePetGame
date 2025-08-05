using Enums;
using Services;
using UnityEngine;

namespace FSM.States
{
    public class StunnedState : IState
    {
        private readonly CharacterFSM _fsm;
        private readonly Stuner _stunService;

        private float _stunDuration = 3f;
        private float _timer;

        public StunnedState(CharacterFSM fsm, GameObject StunnedIcon, Stuner stunService)
        {
            _fsm = fsm;
            _stunService = stunService;
        }

        public void Enter()
        {
            _stunService.ApplyStun(_fsm.gameObject, _fsm.GetComponent<Rigidbody>());
        }

        public void Update()
        {
            _timer += Time.deltaTime;

            if (_timer >= _stunDuration)
            {
                _fsm.ChangeToState(CharacterState.Idle);
            }
        }

        public void Exit()
        {
            _stunService.RemoveStun(_fsm.gameObject, _fsm.GetComponent<Rigidbody>());
            _timer = 0f;
        }
    }
}