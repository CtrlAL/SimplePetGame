using UnityEngine;

namespace Services
{
    public class PlayerInstanseHandler
    {
        private static GameObject _instance;
        public static GameObject Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindWithTag("Player");
                }

                return _instance;
            }
        }
    }
}

