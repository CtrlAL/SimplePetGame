using Enums;
using System;
using UnityEngine;
using Zenject;

namespace Services.Sound
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class SoundManager : MonoBehaviour, IInitializable
    {

        [SerializeField] private Sound[] _soundList;

        private AudioSource _soundSource;

        public void Initialize()
        {
            _soundSource = GetComponent<AudioSource>();
        }

        public void PlaySound(int volume, SoundType soundType)
        {
            var clip = _soundList[(int)soundType];
            _soundSource.PlayOneShot(clip.sound, volume);
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
