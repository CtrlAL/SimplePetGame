using ScriptableObjects;
using Services.Interfaces;
using System.Collections.Concurrent;
using UnityEngine;
using Zenject;

namespace Services
{
    public class StunEffectPool : IStunEffectPool
    {
        [Inject] private readonly DiContainer _diContainer;

        [Inject] private CharacterVFX _characterVFX;

        [Inject] private PoolingSettings _poolingSettings;

        private ConcurrentQueue<ParticleSystem> _particleSystems = new();

        public ParticleSystem SpawnObject()
        {
            if (_particleSystems.TryDequeue(out var effect))
            {
                return effect;
            }
            else
            {
                return _diContainer.InstantiatePrefabForComponent<ParticleSystem>(_characterVFX.StunEffect);
            }
        }

        public void ReturnToPool(ParticleSystem particleSystem)
        {
            if (particleSystem == null)
            {
                return;
            }

            if (_poolingSettings.StunEffectPoolSizeLimit >= _particleSystems.Count)
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
