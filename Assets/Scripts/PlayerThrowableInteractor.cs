using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowableInteractor : MonoBehaviour
{
    [SerializeField]
    private GameObject _throwablesSlot;

    private List<GameObject> _allowThrowables;

    private GameObject _pickedObject;

    public void Awake()
    {
        PlayerInputProvider.GetInputActions().Inputs.Pickup.performed += Pickup;
        _allowThrowables = new List<GameObject>();
    }

    private void Pickup(InputAction.CallbackContext context)
    {
        var player = PlayerInstanse.Instance;

        if (_throwablesSlot == null || player == null || _allowThrowables.Count == 0)
            return;

        var closestThrowable = _allowThrowables
            .OrderBy(go => Vector3.Distance(player.transform.position, go.transform.position))
            .FirstOrDefault();

        if (closestThrowable != null)
        {
            closestThrowable.transform.position = _throwablesSlot.transform.position;
            closestThrowable.transform.SetParent(_throwablesSlot.transform);
            _pickedObject = closestThrowable;
        }
    }

    private void Put(InputAction.CallbackContext context)
    {
    }

    private void Throw(InputAction.CallbackContext context)
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Throwable"))
        {
            _allowThrowables.Add(other.gameObject);
        }
    }

    public void OnDestroy()
    {
        PlayerInputProvider.GetInputActions().Inputs.Pickup.performed -= Pickup;
    }
}
