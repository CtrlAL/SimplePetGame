using FSM;
using FSM.States.CharacterStates;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Services.EventTriggers 
{
    public class EnemyDelayedKickTrigger : MonoBehaviour
    {
        [Inject] private CharacterFSM _fsm;

        [Inject] private KickEventPublisher _kickEventPublisher;

        [Inject] private EnemyStatsSO _stats;

        [Inject] private IFatigue _fatigueService;

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
            yield return new WaitForSeconds(_stats.DelayBeforeKick);

            if (PlayerInstanseHandler.Instance != null && _fsm.GetCurrentState() is IdleState)
            {
                var knockbackMultiplier = _fatigueService.GetKnockbackMultiplier();
                _kickEventPublisher.PublishEnemyKickEvent(gameObject, PlayerInstanseHandler.Instance, _stats.KickPower * knockbackMultiplier);
            }
        }
    }
}


