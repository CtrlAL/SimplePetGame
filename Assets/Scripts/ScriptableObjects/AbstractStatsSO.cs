using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    public abstract class AbstractStatsSO : ScriptableObject
    {
        public abstract float MoveSpeed { get; }
        public abstract float JumpForce { get; }
        public abstract float KickPower { get; }
        public abstract float Fatigue { get; }
    }
}
