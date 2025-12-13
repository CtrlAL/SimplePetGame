using System;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class OutlinePresenter : IInitializable, IDisposable
    {
        [Inject] private readonly ThrowableInteractionView _view;
        private readonly CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _view.OnObjectEnteredRange.Subscribe(go => OnEnterRange(go))
                .AddTo(_compositeDisposable);
            _view.OnObjectExitedRange.Subscribe(go => OnExitRange(go)).
                AddTo(_compositeDisposable);
        }

        public void OnEnterRange(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Outline>(out Outline outline))
            {
                outline.enabled = true;
            }
        }

        public void OnExitRange(GameObject gameObject)
        {
            if (gameObject.TryGetComponent<Outline>(out Outline outline))
            {
                outline.enabled = true;
            }
        }

        public void Dispose() 
        {
            _compositeDisposable?.Dispose();
        }
    }
}
