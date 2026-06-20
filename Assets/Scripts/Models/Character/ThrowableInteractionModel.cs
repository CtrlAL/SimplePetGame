using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Models
{
    public class ThrowableInteractionModel : IDisposable
    {
        private readonly ReactiveProperty<GameObject> _pickedObject = new();

        public IReactiveProperty<GameObject> PickedObject => _pickedObject;

        public HashSet<GameObject> AllowedThrowables { get; } = new();

        public bool IsHolding => _pickedObject.Value != null;

        public void Dispose()
        {
            _pickedObject?.Dispose();
            AllowedThrowables.Clear();
        }
    }
}
