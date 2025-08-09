using FSM.States.CharacterStates;
using Helpers;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using UnityEngine;

namespace Services
{
    public class Mover : IMover
    {
        private readonly MoveEventPublisher _moveEventPublisher;

        public Mover(MoveEventPublisher moveEventPublisher)
        {
            _moveEventPublisher = moveEventPublisher;
            _moveEventPublisher.MoveEvent += Move;
            _moveEventPublisher.JumpEvent += Jump;
        }

        public event Action OnJumped;

        public event Action OnMoved;

        public void Jump(JumpEventArgs args)
        {
            var gameObject = args.FSM.GameObject;
            var fsm = args.FSM;
            var rb = args.Rigidbody;

            if (GameHelpers.IsGrounded(gameObject) && fsm.GetCurrentState() is IdleState)
            {
                rb.AddForce(Vector3.up * args.JumpForce, ForceMode.Impulse);

                OnJumped?.Invoke();
            }
        }

        public void Move(MoveEventArgs args)
        {
            var input = args.Input;
            var objectForMove = args.FSM.GameObject;
            var fsm = args.FSM;
            var rb = args.Rigidbody;

            if (rb != null && fsm.GetCurrentState() is IdleState)
            {
                Vector3 movement = new Vector3(input.x, 0f, input.y);

                rb.AddForce(movement * args.MoveSpeed, ForceMode.Force);
                Rotation(objectForMove, movement, args.RotationSpeed);

                OnMoved?.Invoke();
            }
        }

        public void Rotation(GameObject objectForMove, Vector3 movement, float rotationSpeed)
        {
            var targetRotation = Quaternion.LookRotation(movement, Vector3.up);

            objectForMove.transform.rotation = Quaternion.Slerp(
                objectForMove.transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        public void Dispose()
        {
            _moveEventPublisher.MoveEvent -= Move;
            _moveEventPublisher.JumpEvent -= Jump;
        }
    }
}
