using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class AnimationEventPublisher : MonoBehaviour
    {
        private static AnimationEventPublisher _instance;

        public static AnimationEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<AnimationEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject publisherObject = new GameObject("AnimationEventPublisher");
                        _instance = publisherObject.AddComponent<AnimationEventPublisher>();
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
        }

        public event EventHandler WaveAnimationEnded;

        public event EventHandler WaveAnimationStarted;

        public void PublisWaveAnimationEnded()
        {
            WaveAnimationEnded?.Invoke(this, new());
        }

        public void PublisWaveAnimationStarted()
        {
            WaveAnimationStarted?.Invoke(this, new());
        }
    }
}

