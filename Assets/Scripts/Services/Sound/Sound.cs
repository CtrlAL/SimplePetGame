using System;
using UnityEngine;

namespace Services.Sound
{
    [Serializable]
    public struct Sound
    {
        [HideInInspector] public string name;
        [SerializeField] public AudioClip sound;
    }
}
