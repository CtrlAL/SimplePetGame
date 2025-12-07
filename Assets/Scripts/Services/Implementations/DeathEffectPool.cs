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

        private ConcurrentQueue<ParticleSystem> _particleSystems = new(); 

        public ParticleSystem SpawnObject() 
        {
            if (_particleSystems.TryDequeue(out var effect) && effect != null)
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
            if (particleSystem == null)
            {
                return;
            }

            if (_poolingSettings.DeathEffectPoolSizeLimit >= _particleSystems.Count)
            {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
                _particleSystems.Enqueue(particleSystem);
            }
            else
            {
                GameObject.Destroy(particleSystem);
            }
        }
    }
}
