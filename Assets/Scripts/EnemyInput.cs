using UnityEngine;

public class EnemyInput : MonoBehaviour, IMoveInput
{
    private GameObject _playerObject;
    public Vector2 MoveInput => GetMoveInput();

    public bool JumpPerformed => false;

    public void Awake()
    {
        _playerObject = GameObject.FindWithTag("Player");
    }

    private Vector2 GetMoveInput()
    {
        if (_playerObject != null)
        {
            var targetPosition = _playerObject.transform.position;
            float distance = Vector3.Distance(transform.position, targetPosition);
            Vector3 direction = (targetPosition - transform.position).normalized;

            return new Vector2(direction.x, direction.z);
        }
        else
        {
            return Vector2.zero;
        }
    }
}
