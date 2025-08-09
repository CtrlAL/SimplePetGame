using FSM;
using FSM.States.CharacterStates;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services.EventTriggers
{
    public class PlayerRadialKickTrigger : MonoBehaviour
    {
        [Inject] IPlayerInputProvider _playerInputProvider;

        [Inject] KickEventPublisher _kickEventPublisher;

        [Inject] SoundEventPublisher _soundEventPublisher;

        [Inject] private PlayerStatsSO _playerStats;

        [Inject] private IFatigue _fatigueService;

        [Inject] private CharacterFSM _fsm;

        public void FixedUpdate()
        {
            if (_playerInputProvider?.Inputs.Kick.IsPressed() == true && _fsm?.GetCurrentState() is IdleState)
            {
                Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, _playerStats.KickRadius);

                foreach (var collider in nearbyColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        float knockbackMultiplier = _fatigueService.GetKnockbackMultiplier();
                        _kickEventPublisher.PublishPlayerKickEvent(
                            gameObject,
                            collider.gameObject,
                            _playerStats.KickPower * knockbackMultiplier
                        );

                        PlayerKickSound();
                    }
                }
            }
        }

        // Это точно надо переделать SoundEventPublisher не должен быть в одном месте с KickEventPublisher

        private void PlayerKickSound()
        {
            _soundEventPublisher.PlaySound(Enums.SoundType.PlayerKick, 1);
        }
    }
}