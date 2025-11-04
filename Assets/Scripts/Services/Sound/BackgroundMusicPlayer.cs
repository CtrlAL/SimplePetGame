using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using System.Threading;
using Zenject;

namespace Services.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusicPlayer : MonoBehaviour, IInitializable, IDisposable
    {
        [Inject] BackgroundSounds _sounds;
        [SerializeField] private bool _shuffle = false;

        private AudioSource _audioSource;
        private List<int> _playOrder = new();
        private int _currentTrackIndex;
        private CancellationTokenSource _cts = new();

        public void Initialize()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;

            if (!Application.isPlaying) return;
            BuildPlayOrder();
            if (_playOrder.Count > 0)
                PlayTrack(0);
        }

        public void SwitchToTrack(BackgroundSoundType type)
        {
            int index = (int)type;
            if (index < 0 || index >= _sounds.SoundList.Length || _sounds.SoundList[index].sound == null)
                return;

            if (_playOrder.Count != GetNonNullTrackCount())
                BuildPlayOrder();

            int orderIndex = _playOrder.IndexOf(index);
            if (orderIndex == -1)
            {
                orderIndex = _playOrder.Count;
                _playOrder.Add(index);
            }

            PlayTrack(orderIndex);
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }

        private void BuildPlayOrder()
        {
            _playOrder.Clear();
            for (int i = 0; i < _sounds.SoundList.Length; i++)
            {
                if (_sounds.SoundList[i].sound != null)
                    _playOrder.Add(i);
            }

            if (_shuffle && _playOrder.Count > 1)
                Shuffle(_playOrder);
        }

        private int GetNonNullTrackCount()
        {
            int count = 0;

            foreach (var sound in _sounds.SoundList)
            {
                if (sound.sound != null)
                {
                    count++;
                }
                    
            }
                
            return count;
        }

        private void PlayTrack(int orderIndex)
        {
            if (_playOrder.Count == 0)
            {
                return;
            }

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();

            _currentTrackIndex = orderIndex % _playOrder.Count;
            int soundIndex = _playOrder[_currentTrackIndex];
            AudioClip clip = _sounds.SoundList[soundIndex].sound;

            if (clip == null) return;

            _audioSource.clip = clip;
            _audioSource.Play();

            _ = WaitAndPlayNextAsync(_cts.Token);
        }

        private async UniTask WaitAndPlayNextAsync(CancellationToken token)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_audioSource.clip.length), cancellationToken: token);
                PlayTrack(_currentTrackIndex + 1);
            }
            catch (OperationCanceledException){}
        }

        private void Shuffle(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}