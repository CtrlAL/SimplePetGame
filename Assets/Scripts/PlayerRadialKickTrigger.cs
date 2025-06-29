using Assets.Scripts.FSM.States.CharacterStates;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerRadialKickTrigger : MonoBehaviour
    {
        [SerializeField]
        private CharacterFSM _fsm;

        [SerializeField]
        private PlayerStatsSO _playerStats;

        [SerializeField]
        private float kickRadius = 1.5f;

        private PlayerInputActions _input;

        public void Awake()
        {
            _input = PlayerInputProvider.Inputs;
        }

        public void FixedUpdate()
        {
            if (_input.Inputs.Kick.IsPressed() && _fsm.GetCurrentState() is IdleState)
            {
                Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, kickRadius);

                foreach (var collider in nearbyColliders)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        if (collider.TryGetComponent<Fatigue>(out var fatigue))
                        {
                            float knockbackMultiplier = fatigue.GetKnockbackMultiplier();
                            KickEventPublisher.Instance.PublishKickEvent(
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