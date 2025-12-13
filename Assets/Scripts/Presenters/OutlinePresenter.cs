using Helpers;
using Models;
using Services;
using System;
using System.Linq;
using UniRx;
using UnityEngine;
using Views;
using Zenject;

namespace Presenters
{
    public class OutlinePresenter : IInitializable, IDisposable, IFixedTickable
    {
        [Inject] private readonly ThrowableInteractionModel _interactionModel;
        [Inject] private readonly ThrowableInteractionView _view;
        private readonly CompositeDisposable _compositeDisposable = new();

        public void Initialize()
        {
            _view.OnObjectExitedRange.Subscribe(go => OnExitRange(go)).
                AddTo(_compositeDisposable);
        }

        public void FixedTick()
        {
            var position = PlayerInstanseHandler.Instance.transform.position;
            var listObjects = _interactionModel.AllowedThrowables.ToList();
            var closest = _interactionModel.AllowedThrowables
                .Where(go => go != null && GameHelpers.IsGrounded(go))
                .Where(go => go.TryGetComponent<Outline>(out var _))
                .OrderBy(go => Vector3.Distance(position, go.transform.position))
                .FirstOrDefault();

            listObjects.ForEach(outline => SetOutlineEnabled(outline, false));
            SetOutlineEnabled(closest, true);
        }

        public void Dispose() 
        {
            _compositeDisposable?.Dispose();
        }

        private void OnExitRange(GameObject gameObject)
        {
            SetOutlineEnabled(gameObject, false);
        }

        private void SetOutlineEnabled(GameObject gameObject, bool enabled)
        {
            if (gameObject.TryGetComponent<Outline>(out var outline))
            {
                outline.enabled = enabled;
            }
        }
    }
}
