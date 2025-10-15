using System;
using UniRx;

namespace FSM.States
{
    public interface IState
    {
        public void Enter() { }
        public void Update() { }
        public void Exit() { }

        public IObservable<Unit> OnStateEnter { get; }
        public IObservable<Unit> OnStateExit { get; }
    }
}