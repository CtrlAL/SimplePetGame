using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyStatsSO", menuName = "Game/Enemy Stats", order = 50)]
    public class EnemyStatsSO : AbstractStatsSO
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _kickPower = 20f;
        [SerializeField] private float _fatigue = 20f;
        [SerializeField] private float _fatigueRestoration = 3f;

        public override float MoveSpeed => _moveSpeed;
        public override float JumpForce => _jumpForce;
        public override float KickPower => _kickPower;
        public override float Fatigue => _fatigue;
        public override float FatigueRestoration => _fatigueRestoration;
    }
}
