using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IMoveInput
    {
        Vector2 MoveInput { get; }
        bool JumpPerformed { get; }
    }
}