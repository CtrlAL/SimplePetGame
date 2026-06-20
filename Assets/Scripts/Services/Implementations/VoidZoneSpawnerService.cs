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
        private readonly DiContainer _container;
        private readonly VoidZoneSpawnSettings _settings;
        private readonly LevelSettings _levelSettings;

        private Bounds _platformBounds;
        private Transform _parent;
        private List<Vector3> _activePositions = new();
        private int _currentCount;
        private float _spawnTimer;
        private CancellationTokenSource _spawnCts;

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

            if (!TryGetWorldSize(_settings.Prefub, out var prefabBounds))
            {
                return false;
            }

            Vector3 prefabSize = prefabBounds.size;
            float prefabHalfX = prefabSize.x * 0.5f;
            float prefabHalfZ = prefabSize.z * 0.5f;

            float platformHalfX = _platformBounds.extents.x;
            float platformHalfZ = _platformBounds.extents.z;

            if (prefabHalfX >= platformHalfX || prefabHalfZ >= platformHalfZ)
            {
                Debug.LogWarning("Prefab too large for platform! Spawn may exceed bounds.");
            }

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

                if (!IsPrefabInsidePlatform(worldPoint, prefabBounds)) continue;

                float checkHalfX = prefabHalfX * 0.3f;
                float checkHalfZ = prefabHalfZ * 0.3f;
                Vector3 halfExtents = new Vector3(checkHalfX, 0.1f, checkHalfZ);

                var colliders = Physics.OverlapBox(worldPoint, halfExtents, _parent.rotation);
                var uniqueObjects = new HashSet<GameObject>(colliders.Select(c => c.gameObject));

                if (uniqueObjects.Count > 1) continue;

                float width = prefabBounds.extents.x;
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
                await DestroyAfterDelay(_container.InstantiatePrefab(_settings.PreviewVFX, worldPoint, rot, _parent), _settings.DelayBeforeSpawn, token);
                await DestroyAfterDelay(_container.InstantiatePrefab(_settings.Prefub, worldPoint, rot, _parent), _settings.Lifetime, token);

                _activePositions.Remove(worldPoint);
                _currentCount--;

                return true;
            }

            return false;
        }

        private async UniTask DestroyAfterDelay(GameObject obj, float delay, CancellationToken token)
        {
            if (obj == null) return;
            obj.SetActive(false);
            await Hide(obj, token);
            if (token.IsCancellationRequested || obj == null) return;
            await Show(obj, token);
            if (token.IsCancellationRequested || obj == null) return;
            await UniTask.WaitForSeconds(delay, cancellationToken: token);
            if (token.IsCancellationRequested || obj == null) return;
            await Hide(obj, token);
            if (obj != null) UnityEngine.Object.Destroy(obj);
        }

        private async UniTask Show(GameObject obj, CancellationToken token)
        {
            if (obj == null || token.IsCancellationRequested) return;
            obj.SetActive(true);
            await SetParticlesAlpha(obj, 1f);
        }

        private async UniTask Hide(GameObject obj, CancellationToken token)
        {
            if (obj == null || token.IsCancellationRequested) return;
            await SetParticlesAlpha(obj, 0f);
            if (token.IsCancellationRequested) return;
            await UniTask.WaitForSeconds(2f, cancellationToken: token);
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

        private UniTask SetParticlesAlpha(GameObject target, float alpha)
        {
            if (target == null) return UniTask.CompletedTask;

            var particles = target.GetComponentsInChildren<ParticleSystem>(true);
            if (particles.Length == 0) return UniTask.CompletedTask;

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

            return UniTask.CompletedTask;
        }
    }
}
