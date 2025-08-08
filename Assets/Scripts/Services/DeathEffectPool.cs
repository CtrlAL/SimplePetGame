using Services.Interfaces;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Services
{
    public class DeathEffectPool : IDeathEffectPool
    {
        [Inject] private readonly DiContainer _diContainer;

        [Inject] private CharacterVFX _characterVFX;

        private ConcurrentBag<ParticleSystem> _particleSystems = new(); 

        public ParticleSystem SpawnObject() 
        {
            if (_particleSystems.TryPeek(out var effect))
            {
                return effect;
            }
            else
            {
                return _diContainer.InstantiatePrefabForComponent<ParticleSystem>(_characterVFX.DeathEffect);
            }
        }

        public void ReturnToPool(ParticleSystem particleSystem)
        {
            if (_characterVFX.DeathEffectPoolSizeLimit >= _particleSystems.Count)
            {
                GameObject.Destroy(particleSystem);
            }
            else
            {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
            }
        }
    }
}
