using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class PlayerRadialKickTrigger : MonoBehaviour
    {
        private List<GameObject> _closeObjects = new List<GameObject>();

        private PlayerInputActions _input;

        public void Awake()
        {
            _input = PlayerInputProvider.GetInputActions();
        }

        public void FixedUpdate()
        {
            if (_input.Inputs.Kick.IsPressed())
            {
                _closeObjects.ForEach(go =>
                {
                    KickEventPublisher.Instance.PublishKickEvent(gameObject, go);
                });
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (Helpers.IsEnemy(other.gameObject))
            {
                _closeObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _closeObjects.Remove(other.gameObject);
        }
    }
}

