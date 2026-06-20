using Constants;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class PlayerProvider : IPlayerProvider, IInitializable
    {
        public GameObject Instance { get; private set; }

        public void Initialize()
        {
            Instance = GameObject.FindWithTag(CharacterTags.Player);
        }
    }
}
