using Assets.Scripts;
using Assets.Scripts.FSM.States.CharacterStates;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowableInteractor : MonoBehaviour
{
    [SerializeField]
    private CharacterFSM _fsm;

    [SerializeField]
    private GameObject _throwablesSlot;

    [SerializeField]
    private float _dropDistance = 0.5f;

    [SerializeField]
    private float _throwForce = 1000f;

    private List<GameObject> _allowThrowables;

    private GameObject _pickedObject;

    public void Awake()
    {
        PlayerInputProvider.Inputs.Inputs.Pickup.performed += PickOrPut;
        PlayerInputProvider.Inputs.Inputs.Throw.performed += Throw;
        _allowThrowables = new List<GameObject>();
    }

    private void PickOrPut(InputAction.CallbackContext context)
    {
        if (_fsm.GetCurrentState() is IdleState)
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
            rb.MovePosition(_throwablesSlot.transform.position);
            PinItem(rb, closestThrowable);
        }
    }

    private void Put(InputAction.CallbackContext context)
    {
        if (_pickedObject != null && _pickedObject.TryGetComponent<Rigidbody>(out var rb))
        {
            Vector3 dropPosition = gameObject.transform.position - gameObject.transform.forward * _dropDistance;
            _pickedObject.transform.position = dropPosition;
            _pickedObject.transform.SetParent(null);
            UnpinItem(rb);
        }
    }

    private void Throw(InputAction.CallbackContext context)
    {
        if (_pickedObject != null && _pickedObject.TryGetComponent<Rigidbody>(out var rb) && _fsm.GetCurrentState() is IdleState)
        {
            Vector3 throwDirection = transform.forward.normalized;

            rb.transform.SetParent(null);
            UnpinItem(rb);
            
            rb.AddForce(throwDirection * _throwForce, ForceMode.Impulse);
        }
    }

    private void UnpinItem(Rigidbody rb)
    {
        rb.useGravity = true;
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.transform.position += Vector3.up * 0.1f;

        _pickedObject = null;
    }

    private void PinItem(Rigidbody rb, GameObject gameObject)
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        _pickedObject = gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Helpers.IsThrowable(other.gameObject))
        {
            _allowThrowables.Add(other.gameObject);
        }
    }

    public void OnDestroy()
    {
        PlayerInputProvider.Inputs.Inputs.Pickup.performed -= Pickup;
    }
}
