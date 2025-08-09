using Constants;
using Services.EventPublishers;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class Stuner : IStuner
    {
        [Inject] private StunEventPublisher _stunEventPublisher;

        public void ApplyStun(GameObject target, Rigidbody rigidbody)
        {
            Debug.Log(target.tag);

            if (rigidbody != null)
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;

                string oldTag = target.tag;
                StunDataStorage.StoreOldTag(target, oldTag);
                target.tag = EnvironmentTags.Throwable;

                _stunEventPublisher.PublishCharacterStunedEvent(target);
            }
        }

        public void RemoveStun(GameObject target, Rigidbody rigidbody)
        {
            Debug.Log(target.tag);

            if (rigidbody != null)
            {
                if (StunDataStorage.TryGetOldTag(target, out string oldTag))
                {
                    target.tag = oldTag;
                    StunDataStorage.RemoveOldTag(target);
                }

                _stunEventPublisher.PublishStunStateExitedEvent(target);
            }
        }
    }
}