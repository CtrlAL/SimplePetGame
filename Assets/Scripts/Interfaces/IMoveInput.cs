using UnityEngine;

public interface IMoveInput
{
    Vector2 MoveInput { get; }
    bool JumpPerformed { get; }
}