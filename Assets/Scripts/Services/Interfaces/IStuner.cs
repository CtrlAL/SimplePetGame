using UnityEngine;

namespace Services.Interfaces
{
    public interface IStuner
    {
        void ApplyStun(GameObject target, Rigidbody rigidbody);
        void RemoveStun(GameObject target, Rigidbody rigidbody);
    }
}