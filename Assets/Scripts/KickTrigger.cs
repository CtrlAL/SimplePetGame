using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class KickTrigger : MonoBehaviour
    {
        private float _kickPower = 3f;

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
                    Kick(go, gameObject);
                });
            }
        }

        private void Kick(GameObject kickedObject, GameObject kicker)
        {
            var rb = kickedObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                var kickerTransform = kicker.transform;
                Vector3 direction = (kickedObject.transform.position - kickerTransform.position).normalized;
                rb.AddForce(direction * _kickPower, ForceMode.Impulse);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            _closeObjects.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            _closeObjects.Remove(other.gameObject);
        }
    }
}

