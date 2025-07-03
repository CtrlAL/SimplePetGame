using Assets.Scripts.Constants;
using Assets.Scripts.EventPublishers;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class Stuner
    {
        public void ApplyStun(GameObject target, Rigidbody rigidbody, GameObject StunnedIcon)
        {
            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                //rigidbody.isKinematic = true;
                //rigidbody.useGravity = false;

                string oldTag = target.tag;
                StunDataStorage.StoreOldTag(target, oldTag);

                target.tag = EnvironmentTags.Throwable;
                StunnedIcon?.SetActive(true);

                StunEventPublisher.Instance.PublishCharacterStunedEvent(target);
            }
        }

        public void RemoveStun(GameObject target, Rigidbody rigidbody, GameObject StunnedIcon)
        {
            if (rigidbody != null)
            {
                //rigidbody.isKinematic = false;
                //rigidbody.useGravity = true;

                if (StunDataStorage.TryGetOldTag(target, out string oldTag))
                {
                    target.tag = oldTag;
                    StunDataStorage.RemoveOldTag(target);
                }

                StunnedIcon?.SetActive(false);
                StunEventPublisher.Instance.PublishCharacterStunedEvent(target);
            }
        }
    }
}