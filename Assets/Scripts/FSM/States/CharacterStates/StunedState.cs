using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.FSM.States
{
    public class StunedState : IState
    {
        private readonly CharacterFSM _fsm;

        private readonly GameObject _gameObject;

        private readonly Rigidbody _rigidbody;

        private readonly GameObject _stunedIcon;

        private float _stunDuration = 0f;

        private float _timer;

        private string _oldTag;

        public StunedState(CharacterFSM fsm, GameObject stunedIcon)
        {
            _fsm = fsm;
            _gameObject = _fsm.gameObject;
            _rigidbody = _fsm.GetComponent<Rigidbody>();
            _stunedIcon = stunedIcon;
        }

        public void Enter()
        {
            if (_rigidbody != null)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;
                _rigidbody.freezeRotation = true;
                _oldTag = _fsm.tag;
                _gameObject.tag = "Throwable";
                _stunedIcon.SetActive(true);
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
                _rigidbody.useGravity = true;
                _rigidbody.freezeRotation = false;
            }

            _gameObject.tag = _oldTag;
            _oldTag = string.Empty;
            _stunedIcon.SetActive(false);
        }
    }
}


