using Services.Interfaces;
using UniRx;
using UnityEngine;
using Zenject;

namespace FSM.States
{
    public class StunnedState : ObservableState, IState
    {
        [Inject] private readonly IStuner _stunService;

        [Inject] private readonly Rigidbody _rigidbody;

        private float _stunDuration = 4f;

        private float _timer;

        public void Enter()
        {
            _stunService.ApplyStun(_rigidbody.gameObject, _rigidbody);
            _onStateEnter.OnNext(Unit.Default);
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _stunDuration)
                Exit();
        }

        public void Exit()
        {
            _stunService.RemoveStun(_rigidbody.gameObject, _rigidbody);
            _timer = 0f;

            _onStateExit.OnNext(Unit.Default);
        }
    }
}