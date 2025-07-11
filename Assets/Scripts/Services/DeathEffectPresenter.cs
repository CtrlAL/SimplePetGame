using System;
using System.Collections;
using UnityEngine;


namespace Assets.Scripts.Services
{
    public class DeathEffectPresenter : IDisposable
    {
        private ParticleSystem _particleSystem;
        private DestroyEnemyEventPublisher _destroyEnemyEventPublisher;

        public DeathEffectPresenter()
        {
            _particleSystem = Resources.Load<ParticleSystem>("Prefubs/UI/DeathEffect");

            _destroyEnemyEventPublisher = DestroyEnemyEventPublisher.Instance;

            _destroyEnemyEventPublisher.DestroyEnemy += ShowEffect;
        }

        public void Dispose()
        {
            _destroyEnemyEventPublisher.DestroyEnemy -= ShowEffect;
        }

        private void ShowEffect(object sender, GameObject e)
        {
            var effect = GameObject.Instantiate(_particleSystem.gameObject, e.transform)
                .GetComponent<ParticleSystem>();

            PlayAndDestroy(effect);
        }

        public IEnumerator PlayAndDestroy(ParticleSystem effect)
        {
            effect.Play();
            yield return new WaitForSeconds(_particleSystem.main.duration);
            GameObject.Destroy(effect.gameObject);
        }
    }
}

