using UnityEngine;

namespace ScriptableObjects
{
    public abstract class AbstractStatsSO : ScriptableObject
    {
        public abstract float MoveSpeed { get; }
        public abstract float JumpForce { get; }
        public abstract float KickPower { get; }
        public abstract float Fatigue { get; }
        public abstract float FatigueRestoration { get; }
    }
}
