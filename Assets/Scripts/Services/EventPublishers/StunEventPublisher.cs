using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class StunEventPublisher
    {
        private static StunEventPublisher _instance;

        public static StunEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StunEventPublisher();
                }

                return _instance;
            }
        }

        private StunEventPublisher()
        {
            if (_instance != null)
            {
                return;
            }

            _instance = this;
        }

        public event EventHandler<CharacterStunedEventArgs> CharacterStuned;

        public event EventHandler<CharacterStunedEventArgs> StunStateExited;

        public void PublishCharacterStunedEvent(GameObject character)
        {
            CharacterStuned?.Invoke(this, new(character));
        }

        public void PublishStunStateExitedEvent(GameObject character)
        {
            StunStateExited?.Invoke(this, new(character));
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
