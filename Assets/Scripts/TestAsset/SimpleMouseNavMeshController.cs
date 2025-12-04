using Services;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SimpleMouseNavMeshController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private PlayerInputActions _inputSystem;

    public void FixedUpdate()
    {
        _navMeshAgent.destination = PlayerInstanseHandler.Instance.transform.position;
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit))
        {
            _navMeshAgent.destination = hit.point;
        }
    }
}