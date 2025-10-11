using Enums;
using Services.EventPublishers;
using System;
using Zenject;

namespace Services.Sound
{
    public class SoundEventRouter : IInitializable, IDisposable
    {
        [Inject] private AnimationEventPublisher _animationEventPublisher;
        [Inject] private SoundEventPublisher _soundEventPublisher;

        public void Initialize()
        {
            _animationEventPublisher.WaveAnimationStarted += InvokeWaveAnimationSound;
        }

        private void InvokeWaveAnimationSound()
        {
            _soundEventPublisher.PlaySound(SoundType.WaveAnimationSound);
        }

        public void Dispose()
        {
            _animationEventPublisher.WaveAnimationStarted -= InvokeWaveAnimationSound;
        }
    }
}

