using Services.Interfaces;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace FSM.States
{
    public class StunnedState : IState
    {
        [Inject] private readonly IStuner _stunService;
        [Inject] private readonly Rigidbody _rigidbody;

        private float _stunDuration = 20f;
        private float _timer;

        private readonly Subject<Unit> _onStunEnd = new();
        public IObservable<Unit> OnStunEnd => _onStunEnd;

        public void Enter()
        {
            _stunService.ApplyStun(_rigidbody.gameObject, _rigidbody);
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _stunDuration)
                _onStunEnd.OnNext(Unit.Default);
        }

        public void Exit()
        {
            _stunService.RemoveStun(_rigidbody.gameObject, _rigidbody);
            _timer = 0f;
        }
    }
}