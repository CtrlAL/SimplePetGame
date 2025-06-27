using Assets.Scripts.FSM.States.CharacterStates;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts 
{
    public class DelayedPlayerKickTrigger : MonoBehaviour
    {
        [SerializeField]
        private CharacterFSM _fsm;

        [SerializeField]
        private CharacterStats _characterStats;

        [SerializeField] 
        private float delayBeforeKick = 1f;

        private Coroutine _delayCoroutine;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerInstanse.Instance)
            {
                var rb = other.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    _delayCoroutine = StartCoroutine(DelayedKick());
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject == PlayerInstanse.Instance && _delayCoroutine != null)
            {
                StopCoroutine(_delayCoroutine);
                _delayCoroutine = null;
            }
        }

        private IEnumerator DelayedKick()
        {
            yield return new WaitForSeconds(delayBeforeKick);

            if (PlayerInstanse.Instance != null && _fsm.GetCurrentState() is IdleState)
            {
                KickEventPublisher.Instance.PublishKickEvent(gameObject, PlayerInstanse.Instance, _characterStats.KickPower);
            }
        }
    }
}


