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
                        DontDestroyOnLoad(publisherObject);
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
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public event EventHandler<PlaySoundEventArgs> PlaySoundRequested;

        public event EventHandler<PlaySoundEventArgs> SwitchBackgroundMusicRequested;

        public void PlaySound(SoundType soundType, int vloume = 1)
        {
            PlaySoundRequested?.Invoke(this, new PlaySoundEventArgs(soundType, vloume));
        }
    }

    public class PlaySoundEventArgs : EventArgs
    {
        public SoundType SoundType;

        public int Vloume;

        public PlaySoundEventArgs(SoundType soundType, int volume)
        {
            SoundType = soundType;
            Vloume = volume;
        }
    }
}
