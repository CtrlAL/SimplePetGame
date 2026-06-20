using UnityEngine;

[CreateAssetMenu(fileName = "NewKickImpactSettigns", menuName = "KickImpactSettigns")]
public class KickImpactSettigns : ScriptableObject
{
    public float MinStrongImpact = 100f;

    public float MinWeakImpact = 50f;

    public int WeakHitCountNeeded = 3;
}
