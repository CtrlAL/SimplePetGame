using System;
using UnityEngine;

namespace Assets.Scripts
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

        public void PublishMoveEvent(Vector2 input, GameObject objectForMove, float speed)
        {
            MoveEvent?.Invoke(this, new MoveEventArgs(input, objectForMove, speed));
        }

        public void PublishJumpEvent(GameObject objectForMove, float jumpForce)
        {
            JumpEvent?.Invoke(this, new JumpEventArgs(objectForMove, jumpForce));
        }
    }

    public class MoveEventArgs : EventArgs
    {
        public Vector2 Input;

        public GameObject ObjectForMove;

        public float Speed;

        public MoveEventArgs(Vector2 input, GameObject gameObject, float speed)
        {
            Input = input;
            ObjectForMove = gameObject;
            Speed = speed;
        }
    }

    public class JumpEventArgs : EventArgs
    {
        public GameObject ObjectForJump;

        public float JumpForce;

        public JumpEventArgs(GameObject gameObject, float jumpForce)
        {
            ObjectForJump = gameObject;
            JumpForce = jumpForce;
        }
    }
}