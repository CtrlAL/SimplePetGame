using System;

namespace Services.EventPublishers
{
    public class AnimationEventPublisher
    {
        public event Action WaveAnimationEnded;

        public event Action WaveAnimationStarted;

        public void PublisWaveAnimationEnded()
        {
            WaveAnimationEnded?.Invoke();
        }

        public void PublisWaveAnimationStarted()
        {
            WaveAnimationStarted?.Invoke();
        }
    }
}

