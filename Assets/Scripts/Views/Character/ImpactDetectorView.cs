using UniRx;
using UnityEngine;

namespace Views
{
    public class ImpactDetectorView : MonoBehaviour
    {
        public readonly Subject<float> OnImpactDetected = new();

        private void OnCollisionEnter(Collision collision)
        {
            float impactForce = collision.relativeVelocity.magnitude;
            OnImpactDetected.OnNext(impactForce);
        }
    }
}
