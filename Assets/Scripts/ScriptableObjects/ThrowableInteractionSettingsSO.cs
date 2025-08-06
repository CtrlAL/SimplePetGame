using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ThrowableInteractionSettings", menuName = "ThrowableInteractionSettings")]
    public class ThrowableInteractionSettingsSO : ScriptableObject
    {
        [Header("Physics")]
        public float ThrowForce = 1000f;

        [Header("Placement")]
        public float DropDistance = 0.5f;
    }
}
