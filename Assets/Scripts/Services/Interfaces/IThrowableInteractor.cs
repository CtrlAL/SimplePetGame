
using System;
using UniRx;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IThrowableInteractor
    {
        void Pickup(GameObject target, Transform slot);
        void Throw(Transform ownerTransform);
        void Put(Transform ownerTransform);
        IObservable<Unit> OnObjectThrown { get; }
    }
}
