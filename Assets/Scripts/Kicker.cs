using UnityEngine;

namespace Assets.Scripts
{
    public class Kicker : MonoBehaviour
    {
        private float _kickPower = 3f;
        private void Awake()
        {
            KickEventPublisher.Instance.KickEvent += Kick;
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
                rb.AddForce(direction * _kickPower, ForceMode.Impulse);
            }
        }

        public void OnDestroy()
        {
            KickEventPublisher.Instance.KickEvent -= Kick;
        }
    }
}


