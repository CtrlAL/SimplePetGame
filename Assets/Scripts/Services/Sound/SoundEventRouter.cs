using Enums;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using Zenject;

namespace Assets.Scripts
{
    public class SoundEventRouter : IInitializable, IDisposable
    {
        [Inject] IMover _mover;

        private readonly List<Action> _unsubscribers = new();

        public void Initialize()
        {
            _mover.OnJumped += InvokeJump;

            KickEventPublisher.Instance.PlayerKickEvent += InvokePlayerKick;
            KickEventPublisher.Instance.EnemyKickEvent += InvokeEnemyKick;
            ObjectThrownEventPublisher.Instance.ObjectThrown += InvokeThrow;
            PickupEventPublisher.Instance.ObjetPickuped += InvokePickUp;
            AnimationEventPublisher.Instance.WaveAnimationStarted += InvokeWaveAnimationSound;
        }

        private void InvokeJump()
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.Jump);
        }

        private void InvokeWaveAnimationSound(object sender, EventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.WaveAnimationSound);
        }

        private void InvokePickUp(object sender, EventArgs args)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.PickUp);
        }

        private void InvokeThrow(object sender, EventArgs args)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.Throw);
        }

        private void InvokeEnemyKick(object sender, KickEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.EnemyKick);
        }

        private void InvokePlayerKick(object sender, KickEventArgs e)
        {
            SoundEventPublisher.Instance.PlaySound(SoundType.PlayerKick);
        }

        public void Dispose()
        {
            _mover.OnJumped -= InvokeJump;
            _unsubscribers.ForEach(x => x());

            KickEventPublisher.Instance.PlayerKickEvent -= InvokePlayerKick;
            KickEventPublisher.Instance.EnemyKickEvent -= InvokeEnemyKick;
            ObjectThrownEventPublisher.Instance.ObjectThrown -= InvokeThrow;
            PickupEventPublisher.Instance.ObjetPickuped -= InvokePickUp;
            AnimationEventPublisher.Instance.WaveAnimationStarted -= InvokeWaveAnimationSound;
        }
    }
}

