using UnityEngine;

namespace ScriptableObjects
{
    public abstract class AbstractStats : ScriptableObject
    {
        public abstract float MoveSpeed { get; }
        public abstract float Acceleration { get; }
        public abstract float RotationSpeed { get; }
        public abstract float JumpForce { get; }
        public abstract float KickPower { get; }
        public abstract float Fatigue { get; }
        public abstract float FatigueRestoration { get; }
        public abstract float DelayBeforeKick { get; }
    }
}
