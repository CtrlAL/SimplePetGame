using Models;
using ScriptableObjects;
using Services.Interfaces;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ThrowableInteractor : IThrowableInteractor
    {
        [Inject]
        private ThrowableInteractionModel _throwableModel;

        [Inject]
        private InteractionSettings _settings;

        private Subject<Unit> _onObjectThrown = new();
        public IObservable<Unit> OnObjectThrown => _onObjectThrown;

        public void Pickup(GameObject target, Transform slot)
        {
            if (target == null || !target.TryGetComponent<Rigidbody>(out var rb)) return;

            target.transform.SetParent(slot);
            target.transform.position = slot.position;
            rb.position = slot.position;

            rb.useGravity = false;
            rb.isKinematic = true;

            _throwableModel.PickedObject.Value = target;
        }

        public void Throw(Transform ownerTransform)
        {
            if (_throwableModel.PickedObject.Value == null || !_throwableModel.PickedObject.Value.TryGetComponent<Rigidbody>(out var rb)) return;

            rb.transform.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 throwDirection = ownerTransform.forward;
            rb.AddForce(throwDirection * _settings.ThrowForce, ForceMode.Impulse);

            _throwableModel.PickedObject.Value = null;
            _onObjectThrown.OnNext(Unit.Default);
        }

        public void Put(Transform ownerTransform)
        {
            if (_throwableModel.PickedObject.Value == null || !_throwableModel.PickedObject.Value.TryGetComponent<Rigidbody>(out var rb)) return;

            Vector3 dropPosition = ownerTransform.position - ownerTransform.forward * _settings.DropDistance;
            rb.transform.position = dropPosition;
            rb.transform.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = false;

            _throwableModel.PickedObject.Value = null;
        }
    }
}
