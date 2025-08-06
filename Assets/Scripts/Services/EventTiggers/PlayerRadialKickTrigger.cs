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
        [Inject]
        IPlayerInputProvider _playerInputProvider;

        [Inject]
        private PlayerStatsSO _playerStats;

        [Inject]
        private IFatigue _fatigueService;

        [Inject]
        private CharacterFSM _fsm;

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

        private void PlayerKickSound()
        {
            SoundEventPublisher.Instance.PlaySound(Enums.SoundType.PlayerKick, 1);
        }
    }
}