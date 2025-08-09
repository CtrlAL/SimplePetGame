using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class AnimationEventPublisher : MonoBehaviour
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

