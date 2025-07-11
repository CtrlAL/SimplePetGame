using Assets.Scripts.EventPublishers;
using System;
using System.Collections.Concurrent;
using UnityEngine;


namespace Assets.Scripts.Services
{
    public class StunEffectPresenter : IDisposable
    {
        private ParticleSystem _particleSystem;

        private ConcurrentDictionary<int, ParticleSystem> _effectCash;
        private ConcurrentBag<ParticleSystem> _effectPool;
        private readonly float _offsetMult = 3f;

        public StunEffectPresenter()
        {
            _effectPool = new ConcurrentBag<ParticleSystem>();
            _effectCash = new ConcurrentDictionary<int, ParticleSystem>();
            _particleSystem = Resources.Load<ParticleSystem>("Prefubs/Effects/ParticleStunEffect");

            StunEventPublisher.Instance.CharacterStuned += ShowStunEffect;
            StunEventPublisher.Instance.StunStateExited += HideStunEffect;
        }

        private void ShowStunEffect(object sender, CharacterStunedEventArgs e)
        {
            if (_effectPool.TryPeek(out var effect) && !_effectCash.TryGetValue(e.GetHashCode(), out _))
            {
                effect.transform.SetParent(e.Character.transform);
                //effect.transform.position += Vector3.up;
                effect.gameObject.SetActive(true);
                effect.Play();
            }
            else
            {
                effect = GameObject.Instantiate(_particleSystem, e.Character.transform).GetComponent<ParticleSystem>();
                //effect.transform.position += Vector3.up;
                effect.Play();
            }

            _effectCash.TryAdd(e.Character.GetHashCode(), effect);
        }

        private void HideStunEffect(object sender, CharacterStunedEventArgs e)
        {
            if (_effectCash.TryGetValue(e.Character.GetHashCode(), out var effect))
            {
                effect.Pause();
                effect.gameObject.SetActive(false);
                _effectPool.Add(effect);
            }
        }

        public void Dispose()
        {
            StunEventPublisher.Instance.CharacterStuned -= ShowStunEffect;
            StunEventPublisher.Instance.StunStateExited -= HideStunEffect;
        }
    }
}

