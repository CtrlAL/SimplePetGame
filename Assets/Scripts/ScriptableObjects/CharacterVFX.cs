using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterVFX", menuName = "CharacterVFX")]
public class CharacterVFX : ScriptableObject
{
    public ParticleSystem DeathEffect;

    public ParticleSystem StundEffect;

    public AnimationClip WaveAnimation;

    public int DeathEffectPoolSizeLimit = 10;
}
