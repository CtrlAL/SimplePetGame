using System;
using UnityEngine;

public class MoveEventPublisher : MonoBehaviour
{
    public event EventHandler<MoveEventArgs> MoveEvent;

    public event EventHandler<JumpEventArgs> JumpEvent;

    public void PublishMoveEvent(Vector2 input, GameObject objectForMove)
    {
        MoveEvent?.Invoke(this, new(input, objectForMove));
    }
    public void PublishJumpEvent(GameObject objectForMove)
    {
        JumpEvent?.Invoke(this, new(objectForMove));
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