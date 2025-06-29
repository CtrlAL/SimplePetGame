using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public struct Sound
    {
        [HideInInspector] public string name;
        [SerializeField] public AudioClip sound;
    }
}
