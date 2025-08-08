using UnityEngine;
using System;
using Services.EventPublishers;

namespace Services.Interfaces
{
    public interface IMover : IDisposable
    {
        void Jump(object sender, JumpEventArgs args);
        void Move(object sender, MoveEventArgs args);
        void Rotation(GameObject objectForMove, Vector3 movement, float rotationSpeed);

        event Action OnJumped;

        event Action OnMoved;
    }
}