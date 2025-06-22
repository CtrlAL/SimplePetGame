using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts
{
    public class Mover : MonoBehaviour
    {
        public float _speed = 20f;
        public float _jumpForce = 0.5f;
        private IMoveInput _moveInput;
        private Rigidbody _rb;

        public void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            Move();
            Jump();
        }

        private void Move()
        {
            var input = _moveInput.MoveInput;
            Debug.Log(input);
            Vector3 movement = new Vector3(input.x, 0f, input.y);
            movement = transform.TransformDirection(movement.normalized);

            Debug.Log(movement);

            _rb.AddForce(movement * _speed, ForceMode.Force);
        }
        private void Jump()
        {
            if (_moveInput.JumpPerformed && IsGrounded())
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            }
        }

        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 0.3f);
        }

        public void SetInput(IMoveInput moveInput)
        {
            _moveInput = moveInput;
        }
    }
}
