using Assets.Scripts.EventPublishers;
using System;
using System.Collections.Concurrent;
using UnityEngine;


namespace Assets.Scripts.Services
{
    public class StundEffectPresenter : IDisposable
    {
        private ParticleSystem _particleSystem;

        private ConcurrentDictionary<int, ParticleSystem> _effectCash;
        private ConcurrentBag<ParticleSystem> _effectPool;
        private readonly Vector3 _offset = new Vector3(0, 1f, 0);

        public StundEffectPresenter()
        {
            _effectPool = new ConcurrentBag<ParticleSystem>();
            _effectCash = new ConcurrentDictionary<int, ParticleSystem>();
            _particleSystem = Resources.Load<ParticleSystem>("/Prefubs/Effects/ParticleStunEffect");

            StunEventPublisher.Instance.CharacterStuned += ShowStunEffect;
            StunEventPublisher.Instance.StunStateExited += HideStunEffect;
        }

        private void ShowStunEffect(object sender, CharacterStunedEventArgs e)
        {
            if (_effectPool.TryPeek(out var effect))
            {
                effect.gameObject.transform.SetParent(e.Character.transform);
                effect.gameObject.transform.position += _offset;
                effect.gameObject.SetActive(true);
                effect.Play();
            }
            else
            {
                effect = GameObject.Instantiate(_particleSystem, e.Character.transform).GetComponent<ParticleSystem>();
                effect.gameObject.transform.position += _offset;
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
            StunEventPublisher.Instance.CharacterStuned += ShowStunEffect;
            StunEventPublisher.Instance.StunStateExited += HideStunEffect;
        }
    }
}

