using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyInput : MonoBehaviour, IMoveInput
    {
        private GameObject _playerObject;
        public Vector2 MoveInput => GetMoveInput();
        public bool JumpPerformed => false;

        private Vector2 GetMoveInput()
        {
            if (_playerObject == null)
            {
                var _playerObject = Helpers.FindPlayer();
            }

            if (_playerObject != null)
            {
                var targetPosition = _playerObject.transform.position;
                Vector3 direction = (targetPosition - transform.position).normalized;

                return new Vector2(direction.x, direction.z);
            }

            return Vector2.zero;
        }
    }
}
