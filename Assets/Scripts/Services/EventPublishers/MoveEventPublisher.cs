using FSM;
using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class MoveEventPublisher : MonoBehaviour
    {
        public event Action<MoveEventArgs> MoveEvent;

        public event Action<JumpEventArgs> JumpEvent;

        public event Action ObjectJumped;

        public event Action OjectMoved;

        public void PublishMoveEvent(Vector2 input, CharacterFSM fsm, Rigidbody rigidbody, float speed, float rotationSpeed)
        {
            MoveEvent?.Invoke(new MoveEventArgs(input, fsm, rigidbody, speed, rotationSpeed));
        }

        public void PublishJumpEvent(CharacterFSM fsm, Rigidbody rigidbody, float jumpForce)
        {
            JumpEvent?.Invoke(new JumpEventArgs(fsm, rigidbody, jumpForce));
        }

        public void PublishObjectMoved()
        {
            OjectMoved?.Invoke();
        }

        public void PublishObjectJumped()
        {
            ObjectJumped?.Invoke();
        }
    }

    public class MoveEventArgs : EventArgs
    {
        public Vector2 Input;

        public CharacterFSM FSM;

        public Rigidbody Rigidbody;

        public float MoveSpeed;

        public float RotationSpeed;

        public MoveEventArgs(Vector2 input, CharacterFSM fsm, Rigidbody rigidbody, float speed, float rotationSpeed)
        {
            Input = input;
            FSM = fsm;
            Rigidbody = rigidbody;
            MoveSpeed = speed;
            RotationSpeed = rotationSpeed;
        }
    }

    public class JumpEventArgs : EventArgs
    {
        public CharacterFSM FSM;

        public Rigidbody Rigidbody;

        public float JumpForce;

        public JumpEventArgs(CharacterFSM fsm, Rigidbody rigidbody, float jumpForce)
        {
            JumpForce = jumpForce;
            FSM = fsm;
            Rigidbody = rigidbody;
        }
    }
}