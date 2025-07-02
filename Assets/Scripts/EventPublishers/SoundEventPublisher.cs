using Assets.Scripts.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundEventPublisher : MonoBehaviour
    {
        private static SoundEventPublisher _instance;

        public static SoundEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SoundEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject publisherObject = new GameObject("SoundEventPublisher");
                        _instance = publisherObject.AddComponent<SoundEventPublisher>();
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

        public event EventHandler<PlaySoundEventArgs> PlaySoundRequested;
        public event EventHandler<PlaySoundEventArgs> SwitchBackgroundMusicRequested;

        public void PlaySound(SoundType soundType, int volume = 1)
        {
            PlaySoundRequested?.Invoke(this, new PlaySoundEventArgs(soundType, volume));
        }
    }

    public class PlaySoundEventArgs : EventArgs
    {
        public SoundType SoundType { get; }
        public int Volume { get; }

        public PlaySoundEventArgs(SoundType soundType, int volume)
        {
            SoundType = soundType;
            Volume = volume;
        }
    }
}