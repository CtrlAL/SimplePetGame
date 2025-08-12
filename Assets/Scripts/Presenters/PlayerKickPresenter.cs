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

        [Inject] IKiker _kiker;

        [Inject] SoundEventPublisher _soundEventPublisher;

        [Inject] private PlayerStatsSO _playerStats;

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
                    _kiker.Kick(_transform.gameObject, collider.gameObject, _playerStats.KickPower);
                    OnEnemyKicked();
                }
            }
        }

        private void OnEnemyKicked()
        {
            PlayKickSound();
        }

        private bool CheckKickPresed()
        {
            return _playerInputProvider?.Inputs.Kick.IsPressed() == true;
        }

        private void PlayKickSound()
        {
            _soundEventPublisher.PlaySound(Enums.SoundType.PlayerKick, 1);
        }
    }
}
