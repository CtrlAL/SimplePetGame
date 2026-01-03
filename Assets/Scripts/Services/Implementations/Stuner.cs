using Constants;
using Extensions;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class Stuner : IStuner
    {
        public void ApplyStun(GameObject target, Rigidbody rigidbody)
        {
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

                if (!rigidbody.gameObject.IsBigEnemy())
                {
                    string oldTag = target.tag;
                    StunDataStorage.StoreOldTag(target, oldTag);
                    target.tag = EnvironmentTags.Throwable;
                }
            }
        }

        public void RemoveStun(GameObject target, Rigidbody rigidbody)
        {
            if (rigidbody != null)
            {
                if (StunDataStorage.TryGetOldTag(target, out string oldTag))
                {
                    target.tag = oldTag;
                    StunDataStorage.RemoveOldTag(target);
                }
            }
        }
    }
}