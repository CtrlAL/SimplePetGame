using System.Collections;
using UnityEngine;

namespace Assets.Scripts 
{
    public class EnemyKickTrigger : MonoBehaviour
    {

        private float _kickPower = 3f;
        private float delayBeforeKick = 1f;
        private Coroutine _delayCoroutine;

        private GameObject _player;


        public void Awake()
        {
            _player = Helpers.FindPlayer();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _player)
            {
                var rb = other.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    _delayCoroutine = StartCoroutine(DelayedKick(rb));
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _player && _delayCoroutine != null)
            {
                StopCoroutine(_delayCoroutine);
                _delayCoroutine = null;
            }
        }

        private IEnumerator DelayedKick(Rigidbody other)
        {
            yield return new WaitForSeconds(delayBeforeKick);

            var kicker = gameObject.transform;
            Vector3 direction = (other.transform.position - kicker.position).normalized;
            other.AddForce(direction * _kickPower, ForceMode.Impulse);
        }
    }
}


