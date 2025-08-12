using FSM;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;
using Zenject;
using Extensions;

namespace Presenters
{
    public class PlayerKickPresenter : IFixedTickable
    {
        [Inject] IPlayerInputProvider _playerInputProvider;

        [Inject] KickEventPublisher _kickEventPublisher;

        [Inject] SoundEventPublisher _soundEventPublisher;

        [Inject] private PlayerStatsSO _playerStats;

        [Inject] private IFatigue _fatigueService;

        [Inject] private CharacterFSM _fsm;

        [Inject] private Transform _transform;

        public void FixedTick()
        {
            if (CheckKickPresed() && _fsm.IsIdleState())
            {
                KickNeardyEnemies();
            }
        }

        private void KickNeardyEnemies()
        {
            Collider[] nearbyColliders = Physics.OverlapSphere(_transform.position, _playerStats.KickRadius);

            foreach (var collider in nearbyColliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    float knockbackMultiplier = _fatigueService.GetKnockbackMultiplier();
                    _kickEventPublisher.PublishPlayerKickEvent(
                        _transform.gameObject,
                        collider.gameObject,
                        _playerStats.KickPower * knockbackMultiplier
                    );

                    PlayerKickSound();
                }
            }
        }

        private bool CheckKickPresed()
        {
            return _playerInputProvider?.Inputs.Kick.IsPressed() == true;
        }

        // Это точно надо переделать SoundEventPublisher не должен быть в одном месте с KickEventPublisher

        private void PlayerKickSound()
        {
            _soundEventPublisher.PlaySound(Enums.SoundType.PlayerKick, 1);
        }
    }
}
