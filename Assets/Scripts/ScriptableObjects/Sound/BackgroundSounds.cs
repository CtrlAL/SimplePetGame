using Enums;
using Services.Sound;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundSound", menuName = "Sounds/BackgroundSound")]
public class BackgroundSounds : ScriptableObject
{
    public Sound[] SoundList;

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(BackgroundSoundType));
        Array.Resize(ref SoundList, names.Length);

        for (int i = 0; i < names.Length; i++)
        {
            SoundList[i].name = names[i];
        }
    }
#endif
}
