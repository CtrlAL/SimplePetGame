using UnityEngine;
using Zenject;

namespace Models
{
    public class MoveCharacterModel
    {
        [Inject] public BoxCollider BoxCollider { get; set; }

        [Inject] public Transform Transform { get; private set; }

        [Inject] public Rigidbody Rigidbody { get; private set; }
    }
}