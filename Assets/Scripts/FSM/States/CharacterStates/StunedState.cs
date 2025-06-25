using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    public class StunedState : IState
    {
        private readonly CharacterFSM _fsm;

        private readonly GameObject _gameObject;

        private readonly Rigidbody _rigidbody;

        private float _stunDuration = 3f;

        private float _timer;

        public StunedState(CharacterFSM fsm)
        {
            _fsm = fsm;
            _gameObject = _fsm.gameObject;
            _rigidbody = _fsm.GetComponent<Rigidbody>();
        }

        public void Enter()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _gameObject.tag = "Throwable";
            }
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
            if (_rigidbody != null)
            {
                _rigidbody.isKinematic = false;
            }

            _gameObject.tag = "Enemy";
        }
    }
}


