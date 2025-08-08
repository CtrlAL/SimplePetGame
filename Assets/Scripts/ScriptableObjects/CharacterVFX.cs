using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterVFX", menuName = "CharacterVFX")]
public class CharacterVFX : ScriptableObject
{
    public ParticleSystem DeathEffect;

    public ParticleSystem StunEffect;

    public AnimationClip WaveAnimation;

    public int DeathEffectPoolSizeLimit = 10;

    public int StunEffectPoolSizeLimit = 10;
}
