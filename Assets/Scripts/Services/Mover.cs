using Assets.Scripts.FSM;
using Assets.Scripts.FSM.States.CharacterStates;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Mover : IDisposable
    {
        private readonly float _defaultRatationSpeed = 7f;

        public Mover()
        {
            MoveEventPublisher.Instance.MoveEvent += Move;
            MoveEventPublisher.Instance.JumpEvent += Jump;
        }

        private void Jump(object sender, JumpEventArgs args)
        {
            var gameObject = args.FSM.gameObject;

            if (Helpers.IsGrounded(gameObject) && args.FSM.GetCurrentState() is IdleState)
            {
                args.Rigidbody.AddForce(Vector3.up * args.JumpForce, ForceMode.Impulse);
                MoveEventPublisher.Instance.PublishObjectJumped();
            }
        }

        private void Move(object sender, MoveEventArgs args)
        {
            var input = args.Input;
            var objectForMove = args.FSM.gameObject;

            var rb = objectForMove.GetComponent<Rigidbody>();
            var fsm = objectForMove.GetComponent<CharacterFSM>();
            if (rb != null && fsm.GetCurrentState() is IdleState)
            {
                Vector3 movement = new Vector3(input.x, 0f, input.y);

                rb.AddForce(movement * args.MoveSpeed, ForceMode.Force);
                Rotation(objectForMove, movement);

                MoveEventPublisher.Instance.PublishObjectMoved();
            }
        }

        private void Rotation(GameObject objectForMove, Vector3 movement)
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
