using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    public class KickEventPublisher : MonoBehaviour
    {
        private static KickEventPublisher _instance;
        public static KickEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<KickEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("KickEventPublisher");
                        _instance = singletonObject.AddComponent<KickEventPublisher>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        public event EventHandler<KickEventArgs> PlayerKickEvent;

        public event EventHandler<KickEventArgs> EnemyKickEvent;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PublishPlayerKickEvent(GameObject kicker, GameObject kicked, float kickPower)
        {
            PlayerKickEvent?.Invoke(this, new KickEventArgs(kicker, kicked, kickPower));
        }

        public void PublishEnemyKickEvent(GameObject kicker, GameObject kicked, float kickPower)
        {
            EnemyKickEvent?.Invoke(this, new KickEventArgs(kicker, kicked, kickPower));
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



