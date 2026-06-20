using Constants;
using Extensions;
using Services.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Services
{
    public class Stuner : IStuner
    {
        private Dictionary<int, string> _storedTags = new();

        public void ApplyStun(GameObject target, Rigidbody rigidbody)
        {
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

                if (!rigidbody.gameObject.IsBigEnemy())
                {
                    _storedTags[target.GetInstanceID()] = target.tag;
                    target.tag = EnvironmentTags.Throwable;
                }
            }
        }

        public void RemoveStun(GameObject target, Rigidbody rigidbody)
        {
            if (rigidbody != null)
            {
                int id = target.GetInstanceID();
                if (_storedTags.TryGetValue(id, out string oldTag))
                {
                    target.tag = oldTag;
                    _storedTags.Remove(id);
                }
            }
        }
    }
}
