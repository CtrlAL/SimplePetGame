using System;
using UniRx;

namespace FSM.States
{
    public abstract class ObservableState : IState
    {
        protected readonly Subject<Unit> _onStateEnter = new();

        protected readonly Subject<Unit> _onStateExit = new();

        public IObservable<Unit> OnStateEnter => _onStateEnter;
        public IObservable<Unit> OnStateExit => _onStateExit;
    }
}