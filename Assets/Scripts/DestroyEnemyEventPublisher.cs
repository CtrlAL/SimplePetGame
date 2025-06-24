using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class DestroyEnemyEventPublisher : MonoBehaviour
    {
        private static DestroyEnemyEventPublisher _instance;
        public static DestroyEnemyEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<DestroyEnemyEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("DestroyEnemyEventPublisher");
                        _instance = singletonObject.AddComponent<DestroyEnemyEventPublisher>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        public event EventHandler<GameObject> DestroyEnemy;

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

        public void PublishEvent(GameObject objectForDelete)
        {
            DestroyEnemy?.Invoke(this, objectForDelete);
        }
    }
}

