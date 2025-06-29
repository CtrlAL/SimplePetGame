using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyStatsSO", menuName = "Game/Enemy Stats", order = 50)]
    public class EnemyStatsSO : AbstractStatsSO
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _kickPower = 20f;
        [SerializeField] private float _fatigue = 20f;

        public override float MoveSpeed => _moveSpeed;
        public override float JumpForce => _jumpForce;
        public override float KickPower => _kickPower;
        public override float Fatigue => _fatigue;
    }
}
