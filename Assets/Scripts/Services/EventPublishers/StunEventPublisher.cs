using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class StunEventPublisher
    {
        public event Action<CharacterStunedEventArgs> CharacterStuned;

        public event Action<CharacterStunedEventArgs> StunStateExited;

        public void PublishCharacterStunedEvent(GameObject character)
        {
            CharacterStuned?.Invoke(new(character));
        }

        public void PublishStunStateExitedEvent(GameObject character)
        {
            StunStateExited?.Invoke(new(character));
        }
    }

    public class CharacterStunedEventArgs : EventArgs
    {
        public GameObject Character;
        public CharacterStunedEventArgs(GameObject character)
        {
            Character = character;
        }
    }
}
