using Cysharp.Threading.Tasks;
using ScriptableObjects;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Services
{
    public class VoidZoneSpawnerService : IVoidZoneSpawnerService, IDisposable
    {
        private const float FadeDuration = 0.5f;
        private const float TriggerDelayAfterAppear = 0.5f;

        private readonly DiContainer _container;
        private readonly VoidZoneSpawnSettings _settings;
        private readonly LevelSettings _levelSettings;

        private Bounds _platformBounds;
        private Transform _parent;
        private List<Vector3> _activePositions = new();
        private int _currentCount;
        private float _spawnTimer;
        private CancellationTokenSource _spawnCts;

        private Bounds _prefabBounds;
        private bool _prefabBoundsValid;

        public VoidZoneSpawnerService(DiContainer container, VoidZoneSpawnSettings settings, LevelSettings levelSettings)
        {
            _container = container;
            _settings = settings;
            _levelSettings = levelSettings;
        }

        public void Initialize(Bounds platformBounds, Transform parent)
        {
            _platformBounds = platformBounds;
            _parent = parent;
            _spawnCts = new CancellationTokenSource();
            _prefabBoundsValid = TryGetWorldSize(_settings.Prefub, out _prefabBounds);
        }

        public void Tick()
        {
            if (Time.timeScale == 0 || !_levelSettings.VoidZoneSpawnEnable) return;

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer < _settings.SpawnInterval) return;

            _spawnTimer = 0f;

            if (_currentCount < _settings.SpawnCount)
            {
                _currentCount++;
                TrySpawnAsync(_spawnCts.Token).Forget();
            }
        }

        public void Cleanup()
        {
            _spawnCts?.Cancel();
            _spawnCts?.Dispose();
            _spawnCts = null;
        }

        public void Dispose()
        {
            Cleanup();
        }

        private async UniTask TrySpawnAsync(CancellationToken token)
        {
            bool success = await TrySpawnOne(token);
            if (!success)
            {
                _currentCount--;
            }
        }

        private async UniTask<bool> TrySpawnOne(CancellationToken token)
        {
            float platformTopY = _platformBounds.center.y + _platformBounds.extents.y;

            if (!_prefabBoundsValid) return false;

            Vector3 prefabSize = _prefabBounds.size;
            float prefabHalfX = prefabSize.x * 0.5f;
            float prefabHalfZ = prefabSize.z * 0.5f;

            float platformHalfX = _platformBounds.extents.x;
            float platformHalfZ = _platformBounds.extents.z;

            if (prefabHalfX >= platformHalfX || prefabHalfZ >= platformHalfZ)
                Debug.LogWarning("Prefab too large for platform! Spawn may exceed bounds.");

            float spawnRangeX = Mathf.Max(platformHalfX - prefabHalfX, 0f);
            float spawnRangeZ = Mathf.Max(platformHalfZ - prefabHalfZ, 0f);

            for (int attempt = 0; attempt < 100; attempt++)
            {
                if (token.IsCancellationRequested) return false;

                Vector3 worldPoint = new Vector3(
                    _platformBounds.center.x + UnityEngine.Random.Range(-spawnRangeX, spawnRangeX),
                    platformTopY + _settings.HeightOffset,
                    _platformBounds.center.z + UnityEngine.Random.Range(-spawnRangeZ, spawnRangeZ)
                );

                if (!IsPrefabInsidePlatform(worldPoint, _prefabBounds)) continue;

                float checkHalfX = prefabHalfX * 0.3f;
                float checkHalfZ = prefabHalfZ * 0.3f;
                Vector3 halfExtents = new Vector3(checkHalfX, 0.1f, checkHalfZ);

                var colliders = Physics.OverlapBox(worldPoint, halfExtents, _parent.rotation);
                var uniqueObjects = new HashSet<GameObject>(colliders.Select(c => c.gameObject));

                if (uniqueObjects.Count > 1) continue;

                float width = _prefabBounds.extents.x;
                bool tooClose = false;
                foreach (var pos in _activePositions)
                {
                    if (Vector3.Distance(worldPoint, pos) < width)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                Quaternion rot = Quaternion.FromToRotation(Vector3.up, _parent.up);

                _activePositions.Add(worldPoint);

                await ShowPreviewAsync(worldPoint, rot, token);
                if (token.IsCancellationRequested)
                {
                    _activePositions.Remove(worldPoint);
                    _currentCount--;
                    return false;
                }

                await ShowVoidZoneAsync(worldPoint, rot, token);

                _activePositions.Remove(worldPoint);
                _currentCount--;
                return true;
            }

            return false;
        }

        private async UniTask ShowPreviewAsync(Vector3 pos, Quaternion rot, CancellationToken token)
        {
            var preview = _container.InstantiatePrefab(_settings.PreviewVFX, pos, rot, _parent);

            SetParticlesAlphaInstant(preview, 0f);
            preview.SetActive(true);

            await FadeParticlesAlpha(preview, 0f, 1f, token);
            if (token.IsCancellationRequested || preview == null) return;

            await UniTask.WaitForSeconds(_settings.DelayBeforeSpawn, cancellationToken: token);
            if (token.IsCancellationRequested || preview == null) return;

            await FadeParticlesAlpha(preview, 1f, 0f, token);
            if (token.IsCancellationRequested || preview == null) return;

            await UniTask.WaitForSeconds(FadeDuration, cancellationToken: token);

            if (preview != null) UnityEngine.Object.Destroy(preview);
        }

        private async UniTask ShowVoidZoneAsync(Vector3 pos, Quaternion rot, CancellationToken token)
        {
            var voidZone = _container.InstantiatePrefab(_settings.Prefub, pos, rot, _parent);
            var collider = voidZone.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;

            SetParticlesAlphaInstant(voidZone, 0f);
            voidZone.SetActive(true);

            await FadeParticlesAlpha(voidZone, 0f, 1f, token);
            if (token.IsCancellationRequested || voidZone == null) return;

            await UniTask.WaitForSeconds(TriggerDelayAfterAppear, cancellationToken: token);
            if (token.IsCancellationRequested || voidZone == null) return;

            if (collider != null) collider.enabled = true;

            float remainingLifetime = Mathf.Max(_settings.Lifetime - FadeDuration - TriggerDelayAfterAppear, 0f);
            await UniTask.WaitForSeconds(remainingLifetime, cancellationToken: token);
            if (token.IsCancellationRequested || voidZone == null) return;

            if (collider != null) collider.enabled = false;

            await FadeParticlesAlpha(voidZone, 1f, 0f, token);
            if (token.IsCancellationRequested || voidZone == null) return;

            await UniTask.WaitForSeconds(FadeDuration, cancellationToken: token);

            if (voidZone != null) UnityEngine.Object.Destroy(voidZone);
        }

        private async UniTask FadeParticlesAlpha(GameObject target, float from, float to, CancellationToken token)
        {
            if (target == null) return;

            var particles = target.GetComponentsInChildren<ParticleSystem>(true);
            if (particles.Length == 0) return;

            float elapsed = 0f;

            while (elapsed < FadeDuration)
            {
                if (target == null || token.IsCancellationRequested) return;

                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / FadeDuration);
                float currentAlpha = Mathf.Lerp(from, to, t);

                foreach (var ps in particles)
                {
                    if (ps == null) continue;
                    var main = ps.main;
                    if (main.startColor.mode == ParticleSystemGradientMode.Color)
                    {
                        Color color = main.startColor.color;
                        color.a = currentAlpha;
                        main.startColor = new ParticleSystem.MinMaxGradient(color);
                    }
                }

                await UniTask.Yield(PlayerLoopTiming.Update);
            }
        }

        private void SetParticlesAlphaInstant(GameObject target, float alpha)
        {
            if (target == null) return;

            var particles = target.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in particles)
            {
                if (ps == null) continue;
                var main = ps.main;
                if (main.startColor.mode == ParticleSystemGradientMode.Color)
                {
                    Color color = main.startColor.color;
                    color.a = alpha;
                    main.startColor = new ParticleSystem.MinMaxGradient(color);
                }
            }
        }

        private bool IsPrefabInsidePlatform(Vector3 worldPoint, Bounds prefabBounds)
        {
            float prefabHalfX = prefabBounds.extents.x / _parent.lossyScale.x;
            float prefabHalfZ = prefabBounds.extents.z / _parent.lossyScale.z;

            float leftEdge = worldPoint.x - prefabHalfX;
            float rightEdge = worldPoint.x + prefabHalfX;
            float meshMinX = _platformBounds.center.x - _platformBounds.extents.x;
            float meshMaxX = _platformBounds.center.x + _platformBounds.extents.x;

            if (leftEdge < meshMinX || rightEdge > meshMaxX) return false;

            float frontEdge = worldPoint.z - prefabHalfZ;
            float backEdge = worldPoint.z + prefabHalfZ;
            float meshMinZ = _platformBounds.center.z - _platformBounds.extents.z;
            float meshMaxZ = _platformBounds.center.z + _platformBounds.extents.z;

            if (frontEdge < meshMinZ || backEdge > meshMaxZ) return false;

            return true;
        }

        private bool TryGetWorldSize(GameObject anyObject, out Bounds bounds)
        {
            if (anyObject == null)
            {
                bounds = default;
                return false;
            }

            GameObject temp = UnityEngine.Object.Instantiate(anyObject, Vector3.zero, Quaternion.identity);
            temp.SetActive(true);

            try
            {
                Renderer renderer = temp.GetComponentInChildren<Renderer>(true);
                bounds = renderer.bounds;
                return true;
            }
            catch (Exception)
            {
                bounds = default;
                return false;
            }
            finally
            {
                UnityEngine.Object.Destroy(temp);
            }
        }
    }
}
