using Assets.Scripts.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundEventRouter : MonoBehaviour
    {
        public void Awake()
        {
            KickEventPublisher.Instance.PlayerKickEvent += InvokePlayerKick;
            KickEventPublisher.Instance.EnemyKickEvent += InvokeEnemyKick;
            ObjectThrownEventPublisher.Instance.ObjectThrown += InvokeThrow;
            MoveEventPublisher.Instance.ObjectJumped += InvokeJump;
            PicupEventPublisher.Instance.ObjetPickuped += InvokePickUp;
        }

        private void InvokePickUp(object sender, EventArgs args)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.PickUp);
        }

        private void InvokeThrow(object sender, EventArgs args)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.Throw);
        }

        private void InvokeJump(object sender, EventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.Jump);
        }

        private void InvokeEnemyKick(object sender, KickEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.EnemyKick);
        }

        private void InvokePlayerKick(object sender, KickEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.PlayerKick);
        }
    }
}

