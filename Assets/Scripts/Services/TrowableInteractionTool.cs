using Services.EventPublishers;
using UnityEngine;

namespace Services
{
    public class TrowableInteractionTool
    {
        private GameObject _pickedObject;
        private readonly float _throwForce;
        private readonly float _dropDistance;

        public TrowableInteractionTool(float throwForce, float dropDistance)
        {
            _throwForce = throwForce;
            _dropDistance = dropDistance;
        }

        public bool IsHoldingItem() => _pickedObject != null;

        public void Pickup(GameObject target, Transform slot)
        {
            if (target != null && target.TryGetComponent<Rigidbody>(out var rb))
            {
                target.transform.position = slot.transform.position;
                target.transform.SetParent(slot.transform);
                rb.MovePosition(slot.transform.position);
                PinItem(rb, target);
                PickupEventPublisher.Instance.PublishObjetPickupedvent();
            }
        }

        public void Throw(Transform ownerTransform)
        {
            if (_pickedObject != null && _pickedObject.TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 throwDirection = ownerTransform.forward.normalized;

                rb.transform.SetParent(null);
                UnpinItem(rb);

                rb.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
                ObjectThrownEventPublisher.Instance.PublishEvent();
            }
        }

        public void Put(Transform ownerTransform)
        {
            if (_pickedObject != null && _pickedObject.TryGetComponent<Rigidbody>(out var rb))
            {
                Vector3 dropPosition = ownerTransform.transform.position - ownerTransform.transform.forward * _dropDistance;
                _pickedObject.transform.position = dropPosition;
                _pickedObject.transform.SetParent(null);
                UnpinItem(rb);
            }
        }

        private void UnpinItem(Rigidbody rb)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.transform.position += Vector3.up * 0.1f;
            _pickedObject = null;
        }

        private void PinItem(Rigidbody rb, GameObject gameObject)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
            _pickedObject = gameObject;
        }
    }
}
