using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class KickEventPublisher : MonoBehaviour
    {
        public event Action<KickEventArgs> PlayerKickEvent;

        public event Action<KickEventArgs> EnemyKickEvent;

        public void PublishPlayerKickEvent(GameObject kicker, GameObject kicked, float kickPower)
        {
            PlayerKickEvent?.Invoke(new KickEventArgs(kicker, kicked, kickPower));
        }

        public void PublishEnemyKickEvent(GameObject kicker, GameObject kicked, float kickPower)
        {
            EnemyKickEvent?.Invoke(new KickEventArgs(kicker, kicked, kickPower));
        }
    }

    public class KickEventArgs : EventArgs
    {
        public GameObject Kicker;

        public GameObject Kicked;

        public float KickPower;

        public KickEventArgs(GameObject kicker, GameObject kicked, float kickPower)
        {
            Kicker = kicker;
            Kicked = kicked;
            KickPower = kickPower;
        }
    }
}



