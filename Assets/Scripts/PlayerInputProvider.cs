using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerInputProvider : MonoBehaviour
    {
        private static PlayerInputProvider _instance;
        public static PlayerInputProvider Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("PlayerInputProvider");
                    _instance = go.AddComponent<PlayerInputProvider>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private PlayerInputActions _actions;

        public static PlayerInputActions Inputs => Instance._actions;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            _actions = new PlayerInputActions();
            _actions.Inputs.Enable();
        }

        private void OnDestroy()
        {
            if (_instance == this)
            {
                _actions.Dispose();
                _actions = null;
                _instance = null;
            }
        }
    }
}