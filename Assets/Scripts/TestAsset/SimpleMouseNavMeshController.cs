using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SimpleMouseNavMeshController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public void Start()
    {
        _navMeshAgent.updatePosition = true;
        _navMeshAgent.updateRotation = true;
    }


    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _navMeshAgent.SetDestination(hit.point);
        }
    }
}