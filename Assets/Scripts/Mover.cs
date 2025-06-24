using UnityEngine;

namespace Assets.Scripts
{
    public class Mover : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 20f;

        [SerializeField]
        private float _jumpForce = 0.5f;

        public void Awake()
        {
            MoveEventPublisher.Instance.MoveEvent += Move;
            MoveEventPublisher.Instance.JumpEvent += Jump;
        }

        private void Jump(object sender, JumpEventArgs args)
        {
            if (IsGrounded(args.ObjectForJump))
            {
                var rb = args.ObjectForJump.GetComponent<Rigidbody>();
                rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
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
                movement.y = 0f;

                rb.AddForce(movement * _speed, ForceMode.Force);
            }
        }

        private bool IsGrounded(GameObject gameObject)
        {
            return Physics.Raycast(gameObject.transform.position, Vector3.down, 0.3f);
        }

        public void OnDestroy()
        {
            MoveEventPublisher.Instance.MoveEvent -= Move;
            MoveEventPublisher.Instance.JumpEvent -= Jump;
        }
    }
}
