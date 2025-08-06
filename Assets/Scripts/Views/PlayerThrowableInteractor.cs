using FSM;
using FSM.States.CharacterStates;
using Helpers;
using Services;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Views.Scene
{
    public class PlayerThrowableInteractor : MonoBehaviour
    {
        [Inject]
        private IPlayerInputProvider _playerInputProvider;

        [Inject]
        private CharacterFSM _fsm;

        [SerializeField]
        private GameObject _throwablesSlot;

        [SerializeField]
        private float _dropDistance = 0.5f;

        [SerializeField]
        private float _throwForce = 1000f;

        private HashSet<GameObject> _allowThrowables;

        private GameObject _pickedObject;

        private TrowableInteractionTool _trowableInteractionTool;

        public void Awake()
        {
            _trowableInteractionTool = new TrowableInteractionTool(_throwForce, _dropDistance);
            _allowThrowables = new HashSet<GameObject>();

            _playerInputProvider.Inputs.Pickup.performed += OnPickup;
            _playerInputProvider.Inputs.Throw.performed += OnThrow;
        }

        private void OnPickup(InputAction.CallbackContext context)
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

        private void OnThrow(InputAction.CallbackContext context)
        {
            if (_fsm.GetCurrentState() is IdleState)
            {
                _trowableInteractionTool.Throw(gameObject.transform);
            }
        }

        private void Pickup(InputAction.CallbackContext context)
        {
            var player = PlayerInstanseHandler.Instance;

            if (_throwablesSlot == null || player == null || _allowThrowables.Count == 0)
                return;

            var closestThrowable = _allowThrowables
                .OrderBy(go => Vector3.Distance(player.transform.position, go.transform.position))
                .FirstOrDefault();

            if (closestThrowable != null && closestThrowable.TryGetComponent<Rigidbody>(out var rb))
            {
                _trowableInteractionTool.Pickup(closestThrowable, _throwablesSlot.transform);
            }
        }

        private void Put(InputAction.CallbackContext context)
        {
            _trowableInteractionTool.Put(gameObject.transform);
        }

        private void OnTriggerEnter(Collider other)
        {
            DeleteNullable();

            if (GameHelpers.IsThrowable(other.gameObject) && GameHelpers.IsGrounded(other.gameObject))
            {
                _allowThrowables.Add(other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            DeleteNullable();

            if (GameHelpers.IsThrowable(other.gameObject) && GameHelpers.IsGrounded(other.gameObject))
            {
                _allowThrowables.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            DeleteNullable();

            if (GameHelpers.IsThrowable(other.gameObject) && _allowThrowables.Contains(other.gameObject))
            {
                _allowThrowables.Remove(other.gameObject);
            }
        }

        private void DeleteNullable()
        {
            var toRemove = _allowThrowables?.Where(item => item == null).ToList();
            foreach (var item in toRemove)
            {
                _allowThrowables.Remove(item);
            }
        }

        public void OnDestroy()
        {
            _playerInputProvider.Inputs.Pickup.performed -= Pickup;
            _playerInputProvider.Inputs.Throw.performed -= OnThrow;
        }
    }
}

