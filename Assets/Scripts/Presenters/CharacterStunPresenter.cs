using FSM;
using Zenject;
using Views;
using UniRx;
using System;
using FSM.States;

namespace Presenters
{
    public class CharacterStunPresenter : IInitializable, IDisposable
    {
        private readonly CharacterFSM _fsm;
        private readonly StunnedState _state;
        private readonly StunEffectView _view;
        private readonly int _id;

        private CompositeDisposable _disposables = new();

        public CharacterStunPresenter(CharacterFSM fsm, 
            StunEffectView view, 
            StunnedState stunnedState)
        {
            _fsm = fsm;
            _state = stunnedState;
            _view = view;
            _id = fsm.GameObject.GetInstanceID();
        }

        public void Initialize()
        {
            _state.OnStateEnter
                .Subscribe(state => _view.ShowStunEffect(_id, _fsm.GameObject.transform))
                .AddTo(_disposables);

            _state.OnStateExit
                .Subscribe(state => _view.HideStunEffect(_id))
                .AddTo(_disposables);
        }

        public void Dispose() 
        {
            _view.HideStunEffect(_id);
            _disposables.Dispose();
        }
    }
}
