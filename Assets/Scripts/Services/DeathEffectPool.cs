using ScriptableObjects;
using Services.Interfaces;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

namespace Services
{
    public class DeathEffectPool : IDeathEffectPool
    {
        [Inject] private readonly DiContainer _diContainer;

        [Inject] private CharacterVFX _characterVFX;

        [Inject] private PoolingSettings _poolingSettings;

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
            if (_poolingSettings.DeathEffectPoolSizeLimit >= _particleSystems.Count)
            {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
                _particleSystems.Add(particleSystem);
            }
            else
            {
                GameObject.Destroy(particleSystem);
            }
        }
    }
}
