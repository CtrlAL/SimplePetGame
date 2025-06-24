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
        }

        public void PublishKickEvent(GameObject objectForMove)
        {
            KickEvent?.Invoke(this, new KickEventArgs(objectForMove));
        }
    }

    public class KickEventArgs : EventArgs
    {
        public GameObject Kicker;

        public KickEventArgs(GameObject kicker)
        {
            Kicker = kicker;
        }
    }
}



