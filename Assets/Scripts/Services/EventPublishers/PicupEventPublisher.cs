using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class PickupEventPublisher : MonoBehaviour
    {
        public event Action ObjetPickuped;

        public void PublishObjetPickupedvent()
        {
            ObjetPickuped?.Invoke();
        }
    }
}
