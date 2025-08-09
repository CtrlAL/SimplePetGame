using Cysharp.Threading.Tasks;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Views
{
    public class DeathEffectView
    {
        [Inject] IDeathEffectPool _deathEffectPool;

        public void ShowEffect(GameObject gameObject)
        {
            gameObject.gameObject.SetActive(false);
            var particleSystem = _deathEffectPool.SpawnObject();
            particleSystem.transform.position = gameObject.transform.position;
            CopyMaterial(gameObject, particleSystem);
            PlayAndDestroy(particleSystem).Forget();
        }

        private async UniTaskVoid PlayAndDestroy(ParticleSystem particleSystem)
        {
            particleSystem.gameObject.SetActive(true);
            particleSystem.Play();
            await UniTask.WaitForSeconds(particleSystem.main.duration);
            _deathEffectPool.ReturnToPool(particleSystem);
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
    }
}

