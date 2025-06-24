using System;
using UnityEngine;

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

    public void PublishMoveEvent(Vector2 input, GameObject objectForMove)
    {
        MoveEvent?.Invoke(this, new MoveEventArgs(input, objectForMove));
    }

    public void PublishJumpEvent(GameObject objectForMove)
    {
        JumpEvent?.Invoke(this, new JumpEventArgs(objectForMove));
    }
}

public class MoveEventArgs : EventArgs
{
    public Vector2 Input;

    public GameObject ObjectForMove;

    public MoveEventArgs(Vector2 input, GameObject gameObject)
    {
        Input = input;
        ObjectForMove = gameObject;
    }
}

public class JumpEventArgs : EventArgs
{
    public GameObject ObjectForJump;

    public JumpEventArgs(GameObject gameObject)
    {
        ObjectForJump = gameObject;
    }
}