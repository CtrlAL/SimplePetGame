using System;
using UnityEngine;

namespace Services.EventPublishers
{
    public class DestroyEnemyEventPublisher
    {
        public event Action<GameObject> DestroyEnemy;

        public void PublishEvent(GameObject objectForDelete)
        {
            DestroyEnemy?.Invoke(objectForDelete);
        }
    }
}

