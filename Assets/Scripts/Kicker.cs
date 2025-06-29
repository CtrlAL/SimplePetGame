using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts
{
    public class Kicker : MonoBehaviour
    {
        private void Awake()
        {
            KickEventPublisher.Instance.PlayerKickEvent += Kick;
            KickEventPublisher.Instance.EnemyKickEvent += Kick;
        }

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

        public void OnDestroy()
        {
            KickEventPublisher.Instance.PlayerKickEvent -= Kick;
            KickEventPublisher.Instance.EnemyKickEvent -= Kick;
        }
    }
}


