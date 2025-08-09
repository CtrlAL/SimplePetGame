using Enums;
using System;

namespace Services.EventPublishers
{
    public class SoundEventPublisher
    {
        public event Action<PlaySoundEventArgs> PlaySoundRequested;
        public event Action<PlaySoundEventArgs> SwitchBackgroundMusicRequested;

        public void PlaySound(SoundType soundType, int volume = 1)
        {
            PlaySoundRequested?.Invoke(new PlaySoundEventArgs(soundType, volume));
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