using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Models
{
    public class ThrowableInteractionModel : IDisposable
    {
        public ReactiveProperty<GameObject> PickedObject = new();

        public HashSet<GameObject> AllowedThrowables = new();
        public bool IsHolding => PickedObject.Value != null;

        public void Dispose()
        {
            PickedObject?.Dispose();
        }
    }
}
