using UnityEngine;

[CreateAssetMenu(fileName = "NewKickImpactSettings", menuName = "KickImpactSettings")]
public class KickImpactSettings : ScriptableObject
{
    public float MinStrongImpact = 100f;

    public float MinWeakImpact = 50f;

    public int WeakHitCountNeeded = 3;
}
