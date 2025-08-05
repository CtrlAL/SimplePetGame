using Constants;
using Services.EventPublishers;
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

                string oldTag = target.tag;
                StunDataStorage.StoreOldTag(target, oldTag);
                target.tag = EnvironmentTags.Throwable;

                StunEventPublisher.Instance.PublishCharacterStunedEvent(target);
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

                StunEventPublisher.Instance.PublishStunStateExitedEvent(target);
            }
        }
    }
}