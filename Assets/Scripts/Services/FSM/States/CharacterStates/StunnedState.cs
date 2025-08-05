using Services;
using System;
using UniRx;
using UnityEngine;

namespace FSM.States
{
    public class StunnedState : IState
    {
        private readonly Stuner _stunService;
        private readonly GameObject _gameObject;
        private readonly Rigidbody _rigidbody;

        private float _stunDuration = 3f;
        private float _timer;

        private readonly Subject<Unit> _onStunEnd = new();
        public IObservable<Unit> OnStunEnd => _onStunEnd;

        public StunnedState(Stuner stunService, GameObject gameObject, Rigidbody rigidbody)
        {
            _stunService = stunService;
            _gameObject = gameObject;
            _rigidbody = rigidbody;
        }

        public void Enter()
        {
            _stunService.ApplyStun(_gameObject, _rigidbody);
        }

        public void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= _stunDuration)
                _onStunEnd.OnNext(Unit.Default);
        }

        public void Exit()
        {
            _stunService.RemoveStun(_gameObject, _rigidbody);
            _timer = 0f;
        }
    }
}