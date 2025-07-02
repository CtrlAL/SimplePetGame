using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewPlayerStats", menuName = "Game/Player Stats", order = 50)]
    public class PlayerStatsSO : AbstractStatsSO
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _kickPower = 20f;
        [SerializeField] private float _kickRadius = 1.5f;
        [SerializeField] private float _fatigue = 100f;
        [SerializeField] private float _fatigueRestoration = 5f;

        public override float MoveSpeed => _moveSpeed;
        public override float JumpForce => _jumpForce;
        public override float KickPower => _kickPower;
        public float KickRadius => _kickRadius;
        public override float Fatigue => _fatigue;
        public override float FatigueRestoration => _fatigueRestoration;
    }
}
