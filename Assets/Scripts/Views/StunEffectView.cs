using Services.EventPublishers;
using Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Views
{
    public class StunEffectView : IDisposable, IFixedTickable
    {
        private readonly StunEventPublisher _stunEventPublisher;
        private readonly IStunEffectPool _stunEffectPool;

        private ConcurrentDictionary<int, ParticleSystem> _viewedEffects;

        private ConcurrentDictionary<int, GameObject> _stunedCharacters;

        public StunEffectView(StunEventPublisher stunEventPublisher, IStunEffectPool stunEffectPool)
        {
            _stunEffectPool = stunEffectPool;
            _stunEventPublisher = stunEventPublisher;

            _viewedEffects = new ConcurrentDictionary<int, ParticleSystem>();
            _stunedCharacters = new ConcurrentDictionary<int, GameObject>();

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
            _stunedCharacters.TryAdd(key, e.Character);

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
            var key = e.Character.GetHashCode();

            _viewedEffects.TryRemove(key, out var stunEffect);
            _stunedCharacters.TryRemove(key, out _);
            _stunEffectPool.ReturnToPool(stunEffect);
        }

        public void Dispose()
        {
            _stunEventPublisher.CharacterStuned -= ShowStunEffect;
            _stunEventPublisher.StunStateExited -= HideStunEffect;
        }

        public void FixedTick()
        {
            var zip = _stunedCharacters.Zip(_viewedEffects, 
                (character, effect) => (Character: character.Value.transform, Effect: effect.Value));

            foreach (var elem in zip)
            {
                MoveOverObject(elem.Character, elem.Effect);
            }
        }
    }
}

