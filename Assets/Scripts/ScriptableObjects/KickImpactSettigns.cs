using UnityEngine;

[CreateAssetMenu(fileName = "NewKickImpactSettigns", menuName = "KickImpactSettigns")]
public class KickImpactSettigns : ScriptableObject
{
    [SerializeField]
    private float _minStrongImpact = 100f;

    [SerializeField]
    private float _minWeakImpact = 50f;
}
