using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PicupEventPublisher : MonoBehaviour
    {
        private static PicupEventPublisher _instance;
        public static PicupEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PicupEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("PicupEventPublisher");
                        _instance = singletonObject.AddComponent<PicupEventPublisher>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        public event EventHandler ObjetPickuped;

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

        public void PublishObjetPickupedvent()
        {
            ObjetPickuped?.Invoke(this, new());
        }
    }
}
