using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class ObjectThrownEventPublisher : MonoBehaviour
    {
        public event EventHandler ObjectThrown;

        public void PublishEvent()
        {
            ObjectThrown?.Invoke(this, new());
        }
    }
}