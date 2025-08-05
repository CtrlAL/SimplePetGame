using Assets.Scripts.Services.Interfaces;
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
        IPlayerInputProvider _playerInputProvider;

        private PlayerStatsSO _playerStats;

        private IFatigue _fatigueService;

        [SerializeField]
        private CharacterFSM _fsm;

        [Inject]
        public void Constractor(IPlayerInputProvider playerInputProvider, PlayerStatsSO playerStatsSO, IFatigue fatigueService)
        {
            _playerInputProvider = playerInputProvider;
            _playerStats = playerStatsSO;
            _fatigueService = fatigueService;
        }

        public void FixedUpdate()
        {
            if (_playerInputProvider.Inputs.Kick.IsPressed() && _fsm.GetCurrentState() is IdleState)
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