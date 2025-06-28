using Assets.Scripts.Enums;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    public class StunedState : IState
    {
        private readonly CharacterFSM _fsm;
        private readonly Stuner _stunService;
        private readonly GameObject _stunedIcon;

        private float _stunDuration = 3f;
        private float _timer;

        public StunedState(CharacterFSM fsm, GameObject stunedIcon, Stuner stunService)
        {
            _fsm = fsm;
            _stunedIcon = stunedIcon;
            _stunService = stunService;
        }

        public void Enter()
        {
            _stunService.ApplyStun(_fsm.gameObject, _fsm.GetComponent<Rigidbody>(), _stunedIcon);
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
            _stunService.RemoveStun(_fsm.gameObject, _fsm.GetComponent<Rigidbody>(), _stunedIcon);
            _timer = 0f;
        }
    }
}