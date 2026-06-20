using UnityEngine;
using Zenject;

namespace Models
{
    public class MoveCharacterModel
    {
        [Inject] private BoxCollider _boxCollider;
        [Inject] private Transform _transform;
        [Inject] private Rigidbody _rigidbody;

        public BoxCollider BoxCollider => _boxCollider;
        public Transform Transform => _transform;
        public Rigidbody Rigidbody => _rigidbody;
    }
}
