using UnityEngine;


namespace Assets.Scripts.Services
{
    public class CourutineRunner : MonoBehaviour
    {
        private static CourutineRunner _instance;
        public static CourutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CourutineRunner>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("CourutineRunner");
                        _instance = singletonObject.AddComponent<CourutineRunner>();
                        DontDestroyOnLoad(singletonObject);
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
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

