using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class ObjectThrownEventPublisher
    {
        public event Action ObjectThrown;

        public void PublishEvent()
        {
            ObjectThrown?.Invoke();
        }
    }
}