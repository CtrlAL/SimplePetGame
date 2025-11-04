using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "InteractionSettings", menuName = "InteractionSettings")]
    public class InteractionSettings : ScriptableObject
    {
        [Header("Physics")]
        public float ThrowForce = 1000f;

        [Header("Placement")]
        public float DropDistance = 0.5f;
    }
}
