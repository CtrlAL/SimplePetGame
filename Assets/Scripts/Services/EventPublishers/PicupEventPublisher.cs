using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class PickupEventPublisher
    {
        public event Action ObjetPickuped;

        public void PublishObjetPickupedvent()
        {
            ObjetPickuped?.Invoke();
        }
    }
}
