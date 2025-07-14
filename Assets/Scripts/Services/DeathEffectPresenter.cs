using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;


namespace Assets.Scripts.Services
{
    public class DeathEffectPresenter : IDisposable
    {
        private ParticleSystem _particleSystem;
        private DestroyEnemyEventPublisher _destroyEnemyEventPublisher;
        private ConcurrentBag<ParticleSystem> _effectPool;

        private int _poolLimit = 10;

        public DeathEffectPresenter()
        {
            _effectPool = new ConcurrentBag<ParticleSystem>();
            _particleSystem = Resources.Load<ParticleSystem>("Prefubs/Effects/DeathEffect");

            _destroyEnemyEventPublisher = DestroyEnemyEventPublisher.Instance;
            _destroyEnemyEventPublisher.DestroyEnemy += ShowEffect;
        }

        private void ShowEffect(object sender, GameObject e)
        {
            e.gameObject.SetActive(false);
            
            if (!_effectPool.TryPeek(out var effect))
            {
                effect = GameObject.Instantiate(_particleSystem)
                    .GetComponent<ParticleSystem>();
            }

            effect.transform.position = e.transform.position;

            CourutineRunner.Instance.StartCoroutine(PlayAndHide(effect, e.gameObject));
        }

        public IEnumerator PlayAndHide(ParticleSystem effect, GameObject characterObject)
        {
            if (!effect.gameObject.activeSelf)
            {
                effect.gameObject.SetActive(true);
            }

            effect.Play();

            yield return new WaitForSeconds(_particleSystem.main.duration);
            HideEffect(effect);
        }

        private void HideEffect(ParticleSystem effect)
        {
            if (effect != null)
            {
                if (_poolLimit > _effectPool.Count)
                {
                    effect.Pause();
                    effect.gameObject.SetActive(false);
                    _effectPool.Add(effect);
                }
                else
                {
                    GameObject.Destroy(effect);
                }
            }
        }

        public void Dispose()
        {
            _destroyEnemyEventPublisher.DestroyEnemy -= ShowEffect;
        }
    }
}

