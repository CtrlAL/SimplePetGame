using FSM;
using FSM.States.CharacterStates;
using ScriptableObjects;
using Services.EventPublishers;
using System.Collections;
using UnityEngine;
using Views.Scene.Characters;

namespace Services.EventTriggers 
{
    [RequireComponent(typeof(CharacterFSM))]
    public class EnemyDelayedKickTrigger : MonoBehaviour
    {
        [SerializeField]
        private CharacterFSM _fsm;

        [SerializeField]
        private EnemyStatsSO _stats;

        [SerializeField] 
        private float delayBeforeKick = 1f;

        private Coroutine _delayCoroutine;

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerInstanseHandler.Instance && _delayCoroutine == null)
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
            if (other.gameObject == PlayerInstanseHandler.Instance && _delayCoroutine != null)
            {
                StopCoroutine(_delayCoroutine);
                _delayCoroutine = null;
            }
        }

        private IEnumerator DelayedKick()
        {
            yield return new WaitForSeconds(delayBeforeKick);

            if (PlayerInstanseHandler.Instance != null && _fsm.GetCurrentState() is IdleState)
            {
                if(PlayerInstanseHandler.Instance.TryGetComponent<Fatigue>(out var fatigue)) 
                {
                    var knockbackMultiplier = fatigue.GetKnockbackMultiplier();
                    KickEventPublisher.Instance.PublishEnemyKickEvent(gameObject, PlayerInstanseHandler.Instance, _stats.KickPower * knockbackMultiplier);
                }
            }
        }
    }
}


