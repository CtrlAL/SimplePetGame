using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Models
{
    public class ThrowableInteractionModel
    {
        public ReactiveProperty<GameObject> PickedObject;

        public HashSet<GameObject> AllowedThrowables = new();

        public bool IsHolding => PickedObject.Value != null;
    }
}
