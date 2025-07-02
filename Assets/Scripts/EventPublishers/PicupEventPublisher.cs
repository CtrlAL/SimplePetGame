using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PickupEventPublisher : MonoBehaviour
    {
        private static PickupEventPublisher _instance;
        public static PickupEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<PickupEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("PickupEventPublisher");
                        _instance = singletonObject.AddComponent<PickupEventPublisher>();
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
