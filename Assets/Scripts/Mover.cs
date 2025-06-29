using UnityEngine;

namespace Assets.Scripts
{
    public class Mover : MonoBehaviour
    {
        public void Awake()
        {
            MoveEventPublisher.Instance.MoveEvent += Move;
            MoveEventPublisher.Instance.JumpEvent += Jump;
        }

        private void Jump(object sender, JumpEventArgs args)
        {
            if (Helpers.IsGrounded(args.ObjectForJump))
            {
                var rb = args.ObjectForJump.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * args.JumpForce, ForceMode.Impulse);
            }
        }

        private void Move(object sender, MoveEventArgs args)
        {
            var input = args.Input;
            var objectForMove = args.ObjectForMove;

            var rb = objectForMove.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 movement = new Vector3(input.x, 0f, input.y);
                movement = transform.TransformDirection(movement.normalized);

                rb.AddForce(movement * args.MoveSpeed, ForceMode.Force);
            }
        }

        public void OnDestroy()
        {
            MoveEventPublisher.Instance.MoveEvent -= Move;
            MoveEventPublisher.Instance.JumpEvent -= Jump;
        }
    }
}
