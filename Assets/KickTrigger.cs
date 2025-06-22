using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class KickTrigger : MonoBehaviour
    {
        private float _kickPower = 5f;

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
                    var rb = go.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        var kicker = gameObject.transform;
                        Vector3 direction = (go.transform.position - kicker.position).normalized;
                        rb.AddForce(direction * _kickPower, ForceMode.Impulse);
                    }
                });
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            _closeObjects.Add(other.gameObject);

            if (other.CompareTag("Enemy"))
            {
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _closeObjects.Remove(other.gameObject);

            if (other.CompareTag("Enemy"))
            {
                
            }
        }
    }
}

