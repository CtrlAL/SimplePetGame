using Enums;
using Services.EventPublishers;
using System;
using Zenject;

namespace Services.Sound
{
    public class SoundEventRouter : IInitializable, IDisposable
    {
        [Inject] private KickEventPublisher _kickEventPublisher;
        [Inject] private ObjectThrownEventPublisher _objectThrownEventPublisher;
        [Inject] private PickupEventPublisher _pickupEventPublisher;
        [Inject] private AnimationEventPublisher _animationEventPublisher;
        [Inject] private SoundEventPublisher _soundEventPublisher;

        public void Initialize()
        {
            _kickEventPublisher.PlayerKickEvent += InvokePlayerKick;
            _kickEventPublisher.EnemyKickEvent += InvokeEnemyKick;
            _objectThrownEventPublisher.ObjectThrown += InvokeThrow;
            _pickupEventPublisher.ObjetPickuped += InvokePickUp;
            _animationEventPublisher.WaveAnimationStarted += InvokeWaveAnimationSound;
        }

        private void InvokeWaveAnimationSound()
        {
            _soundEventPublisher.PlaySound(SoundType.WaveAnimationSound);
        }

        private void InvokePickUp()
        {
            _soundEventPublisher.PlaySound(SoundType.PickUp);
        }

        private void InvokeThrow()
        {
            _soundEventPublisher.PlaySound(SoundType.Throw);
        }

        private void InvokeEnemyKick(KickEventArgs e)
        {
            _soundEventPublisher.PlaySound(SoundType.EnemyKick);
        }

        private void InvokePlayerKick(KickEventArgs e)
        {
            _soundEventPublisher.PlaySound(SoundType.PlayerKick);
        }

        public void Dispose()
        {
            _kickEventPublisher.PlayerKickEvent -= InvokePlayerKick;
            _kickEventPublisher.EnemyKickEvent -= InvokeEnemyKick;
            _objectThrownEventPublisher.ObjectThrown -= InvokeThrow;
            _pickupEventPublisher.ObjetPickuped -= InvokePickUp;
            _animationEventPublisher.WaveAnimationStarted -= InvokeWaveAnimationSound;
        }
    }
}

