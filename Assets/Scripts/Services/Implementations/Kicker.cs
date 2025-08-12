using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class Kicker : IKiker
    {
        [Inject] private Transform _transform;

        public void Kick(GameObject kicked, float kickPower)
        {
            if (kicked?.TryGetComponent<Rigidbody>(out var rb) == true)
            {
                Vector3 direction = (kicked.transform.position - _transform.position).normalized;
                rb.AddForce(direction * kickPower, ForceMode.Impulse);
            }
        }
    }
}


