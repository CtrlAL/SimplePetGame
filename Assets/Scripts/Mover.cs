using UnityEngine;

namespace Assets.Scripts
{
    public class Mover : MonoBehaviour
    {
        [SerializeField]
        private float _speed = 20f;

        [SerializeField]
        private float _jumpForce = 0.5f;

        private Vector3[] _ofsets = new Vector3[]
        {
            Vector3.zero,
            new Vector3(0.3f, 0, 0.3f),
            new Vector3(-0.3f, 0, 0.3f),
            new Vector3(0.3f, 0, -0.3f),
            new Vector3(-0.3f, 0, -0.3f),
        };

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
            foreach (var offset in _ofsets)
            {
                Vector3 origin = gameObject.transform.position + offset;

                if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 0.3f))
                {
                    return true;
                }
            }

            return false;
        }

        public void OnDestroy()
        {
            MoveEventPublisher.Instance.MoveEvent -= Move;
            MoveEventPublisher.Instance.JumpEvent -= Jump;
        }
    }
}
