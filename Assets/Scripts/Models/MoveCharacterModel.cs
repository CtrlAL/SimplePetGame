using UnityEngine;

namespace Models
{
    public class MoveCharacterModel
    {
        public Transform Transform { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        public MoveCharacterModel(Transform transform, Rigidbody rigidbody)
        {
            Transform = transform;
            Rigidbody = rigidbody;
        }
    }
}
