using Cysharp.Threading.Tasks;
using Services.EventPublishers;
using Services.Interfaces;
using System;
using UnityEngine;
using Zenject;

namespace Views
{
    public class DeathEffectView : IDisposable, IInitializable
    {
        [Inject] IDeathEffectPool _deathEffectPool;

        public void Initialize()
        {
            DestroyEnemyEventPublisher.Instance.DestroyEnemy += ShowEffect;
        }

        private void ShowEffect(GameObject gameObject)
        {
            gameObject.gameObject.SetActive(false);
            var particleSystem = _deathEffectPool.SpawnObject();
            particleSystem.transform.position = gameObject.transform.position;
            CopyMaterial(gameObject, particleSystem);
            PlayAndDestroy(particleSystem).Forget();
        }

        private async UniTaskVoid PlayAndDestroy(ParticleSystem particleSystem)
        {
            particleSystem.Play();
            await UniTask.WaitForSeconds(particleSystem.main.duration);
            GameObject.Destroy(particleSystem);
        }

        private static void CopyMaterial(GameObject gameObject, ParticleSystem particleSystem)
        {
            Renderer eRenderer = gameObject.GetComponent<Renderer>();
            Renderer effectRenderer = particleSystem.GetComponent<Renderer>();
            if (eRenderer != null && effectRenderer != null)
            {
                effectRenderer.material = eRenderer.material;
            }
        }

        public void Dispose()
        {
            DestroyEnemyEventPublisher.Instance.DestroyEnemy -= ShowEffect;
        }
    }
}

