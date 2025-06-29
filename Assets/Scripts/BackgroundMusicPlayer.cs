using Assets.Scripts.Enums;
using System;
using UnityEngine;


namespace Assets.Scripts
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class BackgroundMusicPlayer : MonoBehaviour
    {
        [SerializeField]
        private Sound[] _backgroundSoundList;

        private static BackgroundMusicPlayer _instance;

        private AudioSource _soundSource;

        private void Awake()
        {
            _instance = this;
            _soundSource = GetComponent<AudioSource>();
            _soundSource.loop = true;
            _soundSource.playOnAwake = false;
            SoundEventPublisher.Instance.SwitchBackgroundMusicRequested += SwitchSound;
        }

        private void SwitchSound(object sender, PlaySoundEventArgs args)
        {
            var clip = _instance._backgroundSoundList[(int)args.SoundType];
            _soundSource.clip = clip.sound;
            _soundSource.volume = args.Vloume;
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
