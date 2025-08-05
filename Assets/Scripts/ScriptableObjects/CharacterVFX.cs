using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterVFX", menuName = "CharacterVFX")]
public class CharacterVFX : ScriptableObject
{
    [SerializeField] private ParticleSystem _deathEffect;

    [SerializeField] private ParticleSystem _stundEffect;

    [SerializeField] private AnimationClip _waveAnimation;
}
