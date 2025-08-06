using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class Kicker : IKiker
    {
        public Kicker()
        {
            KickEventPublisher.Instance.PlayerKickEvent += Kick;
            KickEventPublisher.Instance.EnemyKickEvent += Kick;
        }

        public void Kick(object sender, KickEventArgs args)
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

        public void Dispose()
        {
            KickEventPublisher.Instance.PlayerKickEvent -= Kick;
            KickEventPublisher.Instance.EnemyKickEvent -= Kick;
        }
    }
}


