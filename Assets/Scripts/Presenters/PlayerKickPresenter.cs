using FSM;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Zenject;
using Extensions;
using Services.Sound;

namespace Presenters
{
    public class PlayerKickPresenter : IFixedTickable
    {
        [Inject] IPlayerInputProvider _playerInputProvider;

        [Inject] IKiker _kiker;

        [Inject] SoundManager _soundManager;

        [Inject] private PlayerStats _playerStats;

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
                    _kiker.Kick(collider.gameObject, _playerStats.KickPower);
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
            _soundManager.PlaySound(1, Enums.SoundType.PlayerKick);
        }
    }
}
