using FSM;
using FSM.States.CharacterStates;
using ScriptableObjects;
using Services.EventPublishers;
using UnityEngine;
using Views.Scene.Characters;

namespace Services.EventTriggers
{

    [RequireComponent(typeof(CharacterFSM))]
    public class PlayerRadialKickTrigger : MonoBehaviour
    {
        [SerializeField]
        private CharacterFSM _fsm;

        [SerializeField]
        private PlayerStatsSO _playerStats;

        public void FixedUpdate()
        {
            if (PlayerInputProvider.Instance.Inputs.Kick.IsPressed() && _fsm.GetCurrentState() is IdleState)
            {
                Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, _playerStats.KickRadius);

                foreach (var collider in nearbyColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        if (collider.TryGetComponent<Fatigue>(out var fatigue))
                        {
                            float knockbackMultiplier = fatigue.GetKnockbackMultiplier();
                            KickEventPublisher.Instance.PublishPlayerKickEvent(
                                gameObject,
                                collider.gameObject,
                                _playerStats.KickPower * knockbackMultiplier
                            );

                            PlayerKickSound();
                        }
                    }
                }
            }
        }

        private void PlayerKickSound()
        {
            SoundEventPublisher.Instance.PlaySound(Enums.SoundType.PlayerKick, 1);
        }
    }
}