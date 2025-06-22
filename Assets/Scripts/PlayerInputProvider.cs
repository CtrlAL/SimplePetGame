using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerInputProvider : MonoBehaviour
    {
        private static PlayerInputActions _actions;
        public static PlayerInputActions GetInputActions()
        {
            if (_actions == null)
            {
                _actions = new PlayerInputActions();
                _actions.Inputs.Enable();
                return _actions;
            }
            else
            {
                return _actions;
            }
        }
    }
}