using FSM;
using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class MoveEventPublisher : MonoBehaviour
    {
        private static MoveEventPublisher _instance;
        public static MoveEventPublisher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MoveEventPublisher>();

                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject("MoveEventPublisher");
                        _instance = singletonObject.AddComponent<MoveEventPublisher>();
                        DontDestroyOnLoad(singletonObject);
                    }
                }

                return _instance;
            }
        }

        public event EventHandler<MoveEventArgs> MoveEvent;

        public event EventHandler<JumpEventArgs> JumpEvent;

        public event EventHandler ObjectJumped;

        public event EventHandler OjectMoved;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void PublishMoveEvent(Vector2 input, CharacterFSM fsm, Rigidbody rigidbody, float speed)
        {
            MoveEvent?.Invoke(this, new MoveEventArgs(input, fsm, rigidbody, speed));
        }

        public void PublishJumpEvent(CharacterFSM fsm, Rigidbody rigidbody, float jumpForce)
        {
            JumpEvent?.Invoke(this, new JumpEventArgs(fsm, rigidbody, jumpForce));
        }

        public void PublishObjectMoved()
        {
            OjectMoved?.Invoke(this, new());
        }

        public void PublishObjectJumped()
        {
            ObjectJumped?.Invoke(this, new());
        }
    }

    public class MoveEventArgs : EventArgs
    {
        public Vector2 Input;

        public CharacterFSM FSM;

        public Rigidbody Rigidbody;

        public float MoveSpeed;

        public MoveEventArgs(Vector2 input, CharacterFSM fsm, Rigidbody rigidbody, float speed)
        {
            Input = input;
            FSM = fsm;
            Rigidbody = rigidbody;
            MoveSpeed = speed;
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