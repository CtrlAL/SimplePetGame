using Assets.Scripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowableInteractor : MonoBehaviour
{
    [SerializeField]
    private GameObject _throwablesSlot;

    [SerializeField]
    private float _dropDistance = 0.5f;

    private List<GameObject> _allowThrowables;

    private GameObject _pickedObject;

    public void Awake()
    {
        PlayerInputProvider.GetInputActions().Inputs.Pickup.performed += PickOrPut;
        _allowThrowables = new List<GameObject>();
    }

    private void PickOrPut(InputAction.CallbackContext context)
    {
        if (_pickedObject == null)
        {
            Pickup(context);
        }
        else
        {
            Put(context);
        }
    }


    private void Pickup(InputAction.CallbackContext context)
    {
        var player = PlayerInstanse.Instance;

        if (_throwablesSlot == null || player == null || _allowThrowables.Count == 0)
            return;

        var closestThrowable = _allowThrowables
            .OrderBy(go => Vector3.Distance(player.transform.position, go.transform.position))
            .FirstOrDefault();

        if (closestThrowable != null && closestThrowable.TryGetComponent<Rigidbody>(out var rb))
        {
            closestThrowable.transform.position = _throwablesSlot.transform.position;
            closestThrowable.transform.SetParent(_throwablesSlot.transform);
            rb.useGravity = false;
            rb.isKinematic = true;
            _pickedObject = closestThrowable;
        }
    }

    private void Put(InputAction.CallbackContext context)
    {
        if (_pickedObject != null && _pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 dropPosition = gameObject.transform.position - gameObject.transform.forward * _dropDistance;
            _pickedObject.transform.position = dropPosition;
            _pickedObject.transform.SetParent(null);
            rb.useGravity = true;
            rb.isKinematic = false;
            _pickedObject = null;
        }
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if (_pickedObject != null)
        {

        }
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
