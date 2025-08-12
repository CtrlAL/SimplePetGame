using UnityEngine;
using System;
using Models;
using UniRx;

namespace Services.Interfaces
{
    public interface IMover
    {
        void Move(Vector2 input, float speed, float rotationSpeed);
        void Jump(float jumpForce);
        void Rotation(Vector3 movement, float rotationSpeed);
        IObservable<MoveCharacterModel> OnJumped { get; }
        IObservable<MoveCharacterModel> OnMoved { get; }
    }
}