using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class Kicker : IKiker
    {
        public void Kick(GameObject kicker, GameObject kicked, float kickPower)
        {
            var rb = kicked.GetComponent<Rigidbody>();

            if (rb != null)
            {
                var kickerTransform = kicker.transform;
                Vector3 direction = (kicked.transform.position - kickerTransform.position).normalized;
                rb.AddForce(direction * kickPower, ForceMode.Impulse);
            }
        }
    }
}


