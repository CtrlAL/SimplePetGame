using System;
using UniRx;
using UnityEngine;

namespace Views
{
    public class ImpactDetectorView : MonoBehaviour
    {
        private readonly Subject<float> _onImpactDetected = new();
        public IObservable<float> OnImpactDetected => _onImpactDetected;

        private void OnCollisionEnter(Collision collision)
        {
            float impactForce = collision.relativeVelocity.magnitude;
            _onImpactDetected.OnNext(impactForce);
        }

        private void OnDestroy()
        {
            _onImpactDetected?.Dispose();
        }
    }
}
