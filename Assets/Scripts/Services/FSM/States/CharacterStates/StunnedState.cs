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

        public void Enter()
        {
            _stunService.ApplyStun(_rigidbody.gameObject, _rigidbody);
            _onStateEnter.OnNext(Unit.Default);
        }

        public void Exit()
        {
            _stunService.RemoveStun(_rigidbody.gameObject, _rigidbody);
            _onStateExit.OnNext(Unit.Default);
        }
    }
}