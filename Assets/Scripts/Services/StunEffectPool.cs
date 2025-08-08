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

        private ConcurrentBag<ParticleSystem> _particleSystems = new();

        public ParticleSystem SpawnObject()
        {
            if (_particleSystems.TryPeek(out var effect))
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
            if (_poolingSettings.StunEffectPoolSizeLimit >= _particleSystems.Count)
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
