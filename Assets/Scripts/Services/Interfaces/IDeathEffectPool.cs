using UnityEngine;

namespace Services.Interfaces
{
    public interface IDeathEffectPool
    {
        public ParticleSystem SpawnObject();
        public void ReturnToPool(ParticleSystem particleSystem);
    }
}
