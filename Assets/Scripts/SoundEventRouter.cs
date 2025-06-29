using Assets.Scripts.Enums;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class SoundEventRouter : MonoBehaviour
    {
        public void Awake()
        {
            KickEventPublisher.Instance.PlayerKickEvent += InvokePlayerKickChain;
            KickEventPublisher.Instance.EnemyKickEvent += InvokeEnemyKickChain;
            PlayerInputProvider.Inputs.Inputs.Throw.performed += InvokeThrowChain;
            MoveEventPublisher.Instance.JumpEvent += InvokeJumpChain;
        }

        private void InvokeThrowChain(InputAction.CallbackContext context)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.Throw);
        }

        private void InvokeJumpChain(object sender, JumpEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.Jump);
        }

        private void InvokeEnemyKickChain(object sender, KickEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.EnemyKick);
        }

        private void InvokePlayerKickChain(object sender, KickEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.PlayerKick);
        }
    }
}

