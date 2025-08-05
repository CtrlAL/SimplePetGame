using FSM;
using FSM.States.CharacterStates;
using ScriptableObjects;
using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;
using Views.Scene.Characters;
using Zenject;

namespace Services.EventTriggers
{

    [RequireComponent(typeof(CharacterFSM))]
    public class PlayerRadialKickTrigger : MonoBehaviour
    {
        IPlayerInputProvider _playerInputProvider;

        private PlayerStatsSO _playerStats;

        [SerializeField]
        private CharacterFSM _fsm;

        [Inject]
        public void Constractor(IPlayerInputProvider playerInputProvider, PlayerStatsSO playerStatsSO)
        {
            _playerInputProvider = playerInputProvider;
            _playerStats = playerStatsSO;
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