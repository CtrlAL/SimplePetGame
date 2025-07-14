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
            Debug.Log($"Код обеьекта: {e.Character.gameObject.GetHashCode()}");
            if (_effectCash.ContainsKey(e.Character.gameObject.GetHashCode()))
            {
                return;
            }

            if (_effectPool.TryPeek(out var effect))
                effect.transform.SetParent(e.Character.transform);
            else
                effect = GameObject.Instantiate(_particleSystem, e.Character.transform)
                    .GetComponent<ParticleSystem>();
            

            if (!effect.gameObject.activeSelf)
            {
                effect.gameObject.SetActive(true);
            }
            
            MoveOverObject(e.Character.transform, effect);
            effect.Play();

            _effectCash.TryAdd(e.Character.GetHashCode(), effect);
        }

        private static void MoveOverObject(Transform transform, ParticleSystem effect)
        {
            effect.transform.position = transform.position;
            effect.transform.position += Vector3.up;
        }

        private void HideStunEffect(object sender, CharacterStunedEventArgs e)
        {
            Debug.Log($"Код обеьекта: {e.Character.gameObject.GetHashCode()}");
            if (_effectCash.TryGetValue(e.Character.gameObject.GetHashCode(), out var effect))
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

