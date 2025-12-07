using FSM;
using Zenject;
using UniRx;
using System;
using FSM.States;
using Services.Interfaces;
using UnityEngine;

namespace Presenters
{
    public class CharacterStunPresenter : IInitializable, IDisposable, IFixedTickable
    {
        private readonly Transform _transform;
        private readonly CharacterFSM _fsm;
        private readonly StunnedState _state;
        private readonly IStunEffectService _view;
        private readonly int _id;

        private CompositeDisposable _compositeDisposable = new();

        public CharacterStunPresenter(CharacterFSM fsm,
            IStunEffectService view, 
            StunnedState stunnedState,
            Transform transform)
        {
            _fsm = fsm;
            _state = stunnedState;
            _view = view;
            _transform = transform;
            _id = _transform.gameObject.GetInstanceID();
        }

        public void Initialize()
        {
            _state.OnStateEnter
                .Subscribe(state => _view.ShowStunEffect(_id, _transform))
                .AddTo(_compositeDisposable);

            _state.OnStateExit
                .Subscribe(state => _view.HideStunEffect(_id))
                .AddTo(_compositeDisposable);
        }

        public void Dispose() 
        {
            _view.HideStunEffect(_id);
            _compositeDisposable.Dispose();
        }

        public void FixedTick()
        {
            _view.UpdateAllPositions();
        }
    }
}
