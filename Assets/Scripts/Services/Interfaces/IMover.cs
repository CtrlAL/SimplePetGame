using UnityEngine;
using System;
using Services.EventPublishers;
using UniRx;

namespace Services.Interfaces
{
    public interface IMover : IDisposable
    {
        void Jump(object sender, JumpEventArgs args);
        void Move(object sender, MoveEventArgs args);
        void Rotation(GameObject objectForMove, Vector3 movement);

        event Action OnJumped;

        event Action OnMoved;
    }
}