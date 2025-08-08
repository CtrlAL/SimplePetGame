using UnityEngine;

namespace Services.Interfaces
{
    public interface IStunEffectPool
    {
        public ParticleSystem SpawnObject();
        public void ReturnToPool(ParticleSystem particleSystem);
    }
}
