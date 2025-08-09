using Services.EventPublishers;
using Services.Interfaces;
using System;
using System.Collections.Concurrent;
using UnityEngine;

namespace Views
{
    public class StunEffectView : IDisposable
    {
        private readonly StunEventPublisher _stunEventPublisher;
        private readonly IStunEffectPool _stunEffectPool;

        private ConcurrentDictionary<int, ParticleSystem> _viewedEffects;

        public StunEffectView(StunEventPublisher stunEventPublisher, IStunEffectPool stunEffectPool)
        {
            _stunEffectPool = stunEffectPool;
            _stunEventPublisher = stunEventPublisher;

            _viewedEffects = new ConcurrentDictionary<int, ParticleSystem>();

            _stunEventPublisher.CharacterStuned += ShowStunEffect;
            _stunEventPublisher.StunStateExited += HideStunEffect;
        }

        public void ShowStunEffect(CharacterStunedEventArgs e)
        {
            var key = e.Character.GetHashCode();

            if (_viewedEffects.ContainsKey(key))
            {
                HideStunEffect(e);
            }

            var stunEffect = _stunEffectPool.SpawnObject();
            _viewedEffects.TryAdd(key, stunEffect);

            MoveOverObject(e.Character.transform, stunEffect);
            stunEffect.gameObject.SetActive(true);
            stunEffect.Play();
        }

        private static void MoveOverObject(Transform transform, ParticleSystem effect)
        {
            effect.transform.position = transform.position;
            effect.transform.position += Vector3.up;
        }

        public void HideStunEffect(CharacterStunedEventArgs e)
        {
            _viewedEffects.TryRemove(e.Character.GetHashCode(), out var stunEffect);
            _stunEffectPool.ReturnToPool(stunEffect);
        }

        public void Dispose()
        {
            //_stunEventPublisher.CharacterStuned -= ShowStunEffect;
            _stunEventPublisher.StunStateExited -= HideStunEffect;
        }
    }
}

