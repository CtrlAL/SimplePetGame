using Assets.Scripts.Constants;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class Stuner
    {
        public void ApplyStun(GameObject target, Rigidbody rigidbody, GameObject stunedIcon)
        {
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                rigidbody.isKinematic = true;
                rigidbody.useGravity = false;

                string oldTag = target.tag;
                StunDataStorage.StoreOldTag(target, oldTag);

                target.tag = EnvironmentTags.Throwable;
                stunedIcon?.SetActive(true);
            }
        }

        public void RemoveStun(GameObject target, Rigidbody rigidbody, GameObject stunedIcon)
        {
            if (rigidbody != null)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;

                if (StunDataStorage.TryGetOldTag(target, out string oldTag))
                {
                    target.tag = oldTag;
                    StunDataStorage.RemoveOldTag(target);
                }

                stunedIcon?.SetActive(false);
            }
        }
    }
}