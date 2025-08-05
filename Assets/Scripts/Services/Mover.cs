using FSM.States.CharacterStates;
using Helpers;
using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class Mover : IMover
    {
        private readonly float _defaultRatationSpeed = 7f;

        public Mover()
        {
            MoveEventPublisher.Instance.MoveEvent += Move;
            MoveEventPublisher.Instance.JumpEvent += Jump;
        }

        public void Jump(object sender, JumpEventArgs args)
        {
            var gameObject = args.FSM.GameObject;
            var fsm = args.FSM;
            var rb = args.Rigidbody;

            if (GameHelpers.IsGrounded(gameObject) && fsm.GetCurrentState() is IdleState)
            {
                rb.AddForce(Vector3.up * args.JumpForce, ForceMode.Impulse);
                MoveEventPublisher.Instance.PublishObjectJumped();
            }
        }

        public void Move(object sender, MoveEventArgs args)
        {
            var input = args.Input;
            var objectForMove = args.FSM.GameObject;
            var fsm = args.FSM;
            var rb = args.Rigidbody;

            if (rb != null && fsm.GetCurrentState() is IdleState)
            {
                Vector3 movement = new Vector3(input.x, 0f, input.y);

                rb.AddForce(movement * args.MoveSpeed, ForceMode.Force);
                Rotation(objectForMove, movement);

                MoveEventPublisher.Instance.PublishObjectMoved();
            }
        }

        public void Rotation(GameObject objectForMove, Vector3 movement)
        {
            var targetRotation = Quaternion.LookRotation(movement, Vector3.up);

            objectForMove.transform.rotation = Quaternion.Slerp(
                objectForMove.transform.rotation,
                targetRotation,
                _defaultRatationSpeed * Time.deltaTime
            );
        }

        public void Dispose()
        {
            MoveEventPublisher.Instance.MoveEvent -= Move;
            MoveEventPublisher.Instance.JumpEvent -= Jump;
        }
    }
}
