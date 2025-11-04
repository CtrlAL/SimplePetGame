using Enums;
using Services.Sound;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSound", menuName = "Sounds/CharacterSound")]
public class CharacterSounds : ScriptableObject
{
    public Sound[] SoundList;

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref SoundList, names.Length);

        for (int i = 0; i < names.Length; i++)
        {
            SoundList[i].name = names[i];
        }
    }
#endif
}
