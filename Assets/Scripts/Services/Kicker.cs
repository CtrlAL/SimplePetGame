using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class Kicker : IKiker
    {
        private readonly KickEventPublisher _kickEventPublisher;

        public Kicker(KickEventPublisher kickEventPublisher)
        {
            _kickEventPublisher = kickEventPublisher;

            _kickEventPublisher.PlayerKickEvent += Kick;
            _kickEventPublisher.EnemyKickEvent += Kick;
        }

        public void Kick(KickEventArgs args)
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
            _kickEventPublisher.PlayerKickEvent -= Kick;
            _kickEventPublisher.EnemyKickEvent -= Kick;
        }
    }
}


