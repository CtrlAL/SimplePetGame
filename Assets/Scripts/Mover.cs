using Assets.Scripts.FSM.States.CharacterStates;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Mover : IDisposable
    {
        private readonly float _defaultRatationSpeed = 5f;

        public Mover()
        {
            MoveEventPublisher.Instance.MoveEvent += Move;
            MoveEventPublisher.Instance.JumpEvent += Jump;
        }

        private void Jump(object sender, JumpEventArgs args)
        {
            if (Helpers.IsGrounded(args.ObjectForJump) && args.ObjectForJump.TryGetComponent<CharacterFSM>(out var fsm) && fsm.GetCurrentState() is IdleState)
            {
                var rb = args.ObjectForJump.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * args.JumpForce, ForceMode.Impulse);
                MoveEventPublisher.Instance.PublishObjectJumped();
            }
        }

        private void Move(object sender, MoveEventArgs args)
        {
            var input = args.Input;
            var objectForMove = args.ObjectForMove;

            var rb = objectForMove.GetComponent<Rigidbody>();
            var fsm = objectForMove.GetComponent<CharacterFSM>();
            if (rb != null && fsm.GetCurrentState() is IdleState)
            {
                Vector3 movement = new Vector3(input.x, 0f, input.y);
                
                rb.AddForce(movement * args.MoveSpeed, ForceMode.Force);

                var targetRotation = Quaternion.LookRotation(movement, Vector3.up);

                objectForMove.transform.rotation = Quaternion.Slerp(
                    objectForMove.transform.rotation,
                    targetRotation,
                    _defaultRatationSpeed * Time.deltaTime
                );

                MoveEventPublisher.Instance.PublishObjectMoved();
            }
        }

        public void Dispose()
        {
            MoveEventPublisher.Instance.MoveEvent -= Move;
            MoveEventPublisher.Instance.JumpEvent -= Jump;
        }
    }
}
