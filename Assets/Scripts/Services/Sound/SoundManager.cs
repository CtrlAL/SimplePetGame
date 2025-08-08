using Enums;
using Services.EventPublishers;
using System;
using UnityEngine;

namespace Services.Sound
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private Sound[] _soundList;

        private AudioSource _soundSource;

        private void Awake()
        {
            SoundEventPublisher.Instance.PlaySoundRequested += PlaySound;
        }

        private void PlaySound(object sender, PlaySoundEventArgs args)
        {
            var clip = _soundList[(int)args.SoundType];
            _soundSource.PlayOneShot(clip.sound, args.Volume);
        }

        public void PlaySound(int volume, SoundType soundType)
        {
            var clip = _soundList[(int)soundType];
            _soundSource.PlayOneShot(clip.sound, volume);
        }

        private void Start()
        {
            _soundSource = GetComponent<AudioSource>();
        }

        private void OnDestroy()
        {
            SoundEventPublisher.Instance.PlaySoundRequested -= PlaySound;
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            string[] names = Enum.GetNames(typeof(SoundType));
            Array.Resize(ref _soundList, names.Length);

            for (int i = 0; i < names.Length; i++)
            {
                _soundList[i].name = names[i];
            }
        }
#endif
    }
}
