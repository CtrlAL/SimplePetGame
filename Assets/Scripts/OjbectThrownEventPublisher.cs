using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectThrownEventPublisher : MonoBehaviour
    {
        private static ObjectThrownEventPublisher _instance;

        public static ObjectThrownEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ObjectThrownEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject publisherObject = new GameObject(nameof(ObjectThrownEventPublisher));
                        _instance = publisherObject.AddComponent<ObjectThrownEventPublisher>();
                        DontDestroyOnLoad(publisherObject);
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public event EventHandler ObjectThrown;

        public void PublishEvent()
        {
            ObjectThrown?.Invoke(this, new());
        }
    }
}