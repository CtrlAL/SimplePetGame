using Assets.Scripts.Enums;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField]
        private Sound[] _soundList;

        private static SoundManager _instance;

        private AudioSource _soundSource;

        private void Awake()
        {
            _instance = this;
            SoundEventPublisher.Instance.PlaySoundRequested += PlaySound;
        }

        private void PlaySound(object sender, PlaySoundEventArgs args)
        {
            var clip = _instance._soundList[(int)args.SoundType];
            _instance._soundSource.PlayOneShot(clip.sound, args.Volume);
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
