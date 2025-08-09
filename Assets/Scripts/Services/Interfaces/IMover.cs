using UnityEngine;
using System;
using Services.EventPublishers;

namespace Services.Interfaces
{
    public interface IMover : IDisposable
    {
        void Jump(JumpEventArgs args);
        void Move(MoveEventArgs args);
        void Rotation(GameObject objectForMove, Vector3 movement, float rotationSpeed);

        event Action OnJumped;

        event Action OnMoved;
    }
}