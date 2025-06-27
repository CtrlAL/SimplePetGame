using System;
using UnityEngine;

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

        public event EventHandler<KickEventArgs> KickEvent;

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

        public void PublishKickEvent(GameObject kicker, GameObject kicked, float kickPower)
        {
            KickEvent?.Invoke(this, new KickEventArgs(kicker, kicked, kickPower));
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



