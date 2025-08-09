using Enums;
using Services.EventPublishers;
using System;
using UnityEngine;
using Zenject;

namespace Services.Sound
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [SerializeField] private Sound[] _backgroundSoundList;

        [Inject] private SoundEventPublisher _soundEventPublisher;

        private AudioSource _soundSource;

        private void Awake()
        {
            _soundSource = GetComponent<AudioSource>();
            _soundSource.loop = true;
            _soundSource.playOnAwake = false;
            _soundEventPublisher.SwitchBackgroundMusicRequested += SwitchSound;
        }

        private void SwitchSound(PlaySoundEventArgs args)
        {
            var clip = _backgroundSoundList[(int)args.SoundType];
            _soundSource.clip = clip.sound;
            _soundSource.volume = args.Volume;
        }

        void Start()
        {
            if (!Application.isPlaying) return;
            _soundSource.clip = _backgroundSoundList[(int)BackgroundSoundType.DefaultBackground].sound;
            _soundSource.Play();
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(BackgroundSoundType));
            Array.Resize(ref _backgroundSoundList, names.Length);

            for (int i = 0; i < names.Length; i++)
            {
                _backgroundSoundList[i].name = names[i];
            }
        }
#endif
    }
}
