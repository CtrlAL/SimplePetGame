using Enums;
using UnityEngine;
using Zenject;

namespace Services.Sound
{
    [RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
    public class SoundManager : MonoBehaviour, IInitializable
    {
        [Inject] CharacterSounds _sounds;

        private AudioSource _soundSource;

        public void Initialize()
        {
            _soundSource = GetComponent<AudioSource>();
        }

        public void PlaySound(float volume, SoundType soundType)
        {
            var clip = _sounds.SoundList[(int)soundType];
            _soundSource.PlayOneShot(clip.sound, volume);
        }
    }
}
