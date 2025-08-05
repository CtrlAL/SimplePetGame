using Services.EventPublishers;
using System;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IKiker : IDisposable
    {
        private void Kick(object sender, KickEventArgs args)
        {
            var kicked = args.Kicked;
            var kicker = args.Kicker;

            var rb = kicked.GetComponent<Rigidbody>();

            if (rb != null)
            {
                var kickerTransform = kicker.transform;
                Vector3 direction = (kicked.transform.position - kickerTransform.position).normalized;
                rb.AddForce(direction * args.KickPower, ForceMode.Impulse);
            }
        }
    }
}