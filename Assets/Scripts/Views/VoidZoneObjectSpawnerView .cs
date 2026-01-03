using UnityEngine;
using System.Collections.Generic;
using Zenject;
using ScriptableObjects;
using Unity.VisualScripting;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor.Search;

public class VoidZoneObjectSpawnerView : MonoBehaviour
{
    [Inject] DiContainer _container;
    [Inject] LevelSettings _levelSettings;
    [Inject] VoidZoneSpawnSettings _settings;

    private List<Vector3> activePositions = new();
    private Renderer _renderer;
    private Bounds _platformWorldBounds;
    private int _currentCount = 0;
    private CancellationTokenSource _spawnCts;

    private float _spawnTimer = 0f;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _platformWorldBounds = _renderer.bounds;
        _spawnCts = new CancellationTokenSource();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0 || !_levelSettings.VoidZoneSpawnEnable) return;

        _spawnTimer += Time.fixedDeltaTime;
        if (_spawnTimer < _settings.SpawnInterval) return;

        _spawnTimer = 0f;

        if (_currentCount <= _settings.SpawnCount)
        {
            StartSpawnAsync().Forget();
        }
    }

    private async UniTask StartSpawnAsync()
    {
        if (await TrySpawnOne(_spawnCts.Token))
        {
            _currentCount++;
        }
    }

    private async UniTask<bool> TrySpawnOne(CancellationToken cancellationToken = default)
    {
        float platformTopY = _platformWorldBounds.center.y + _platformWorldBounds.extents.y;

        if (!TryGetWorldSize(_settings.Prefub, out var prefabBounds)) 
        {
            return false;
        }

        Vector3 prefabSize = prefabBounds.size;
        float prefabHalfX = prefabSize.x * 0.5f;
        float prefabHalfZ = prefabSize.z * 0.5f;

        float platformHalfX = _platformWorldBounds.extents.x;
        float platformHalfZ = _platformWorldBounds.extents.z;

        if (prefabHalfX >= platformHalfX || prefabHalfZ >= platformHalfZ)
        {
            Debug.LogWarning("Префаб слишком большой для платформы! Спавн может выходить за край.");
        }

        float spawnRangeX = platformHalfX - prefabHalfX;
        float spawnRangeZ = platformHalfZ - prefabHalfZ;

        spawnRangeX = Mathf.Max(spawnRangeX, 0f);
        spawnRangeZ = Mathf.Max(spawnRangeZ, 0f);

        for (int attempt = 0; attempt < 100; attempt++)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            Vector3 localPoint = new Vector3(
                _platformWorldBounds.center.x + Random.Range(-spawnRangeX, spawnRangeX),
                platformTopY,
                _platformWorldBounds.center.z + Random.Range(-spawnRangeZ, spawnRangeZ)
            );

            var width = prefabBounds.extents.x;

            Vector3 worldPoint = localPoint + transform.up * _settings.HeightOffset;

            if (!IsPrefabInsidePlatform(worldPoint, prefabBounds))
            {
                continue;
            }

            float checkHalfX = prefabHalfX * 0.3f;
            float checkHalfZ = prefabHalfZ * 0.3f;
            Vector3 halfExtents = new Vector3(checkHalfX, 0.1f, checkHalfZ);

            var objects = Physics.OverlapBox(worldPoint, halfExtents, transform.rotation)
                .DistinctBy(x => x.gameObject)
                .ToList();

            if (objects.Count > 1)
            {
                continue;
            }

            bool tooClose = false;

            foreach (var pos in activePositions)
            {
                if (Vector3.Distance(worldPoint, pos) < width)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose) continue;

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, transform.up);

            activePositions.Add(worldPoint);
            await DestroyAfterDelay(_container.InstantiatePrefab(_settings.PreviewVFX, worldPoint, rot, transform), _settings.DelayBeforeSpawn);
            await DestroyAfterDelay(_container.InstantiatePrefab(_settings.Prefub, worldPoint, rot, transform), _settings.Lifetime);
            
            RemoveActivePoints(worldPoint);

            return true;
        }

        return false;
    }

    private void RemoveActivePoints(Vector3 pos) {
        activePositions.Remove(pos);
        _currentCount--;
    }

    private async UniTask DestroyAfterDelay(GameObject obj, float delay)
    {
        obj.SetActive(false);

        await Hide(obj);
        await Show(obj);
        await UniTask.WaitForSeconds(delay);
        await Hide(obj);

        if (obj != null) Destroy(obj);
    }

    private async UniTask Show(GameObject obj)
    {
        obj.SetActive(true);
        await SetParticlesAlpha(obj, 1f);
    }

    private async UniTask Hide(GameObject obj)
    {
        await SetParticlesAlpha(obj, 0f);
        await UniTask.WaitForSeconds(2f);
    }

    private bool IsPrefabInsidePlatform(Vector3 worldPoint, Bounds prefabBounds)
    {
        float prefabHalfX = prefabBounds.extents.x / transform.lossyScale.x;
        float prefabHalfZ = prefabBounds.extents.z / transform.lossyScale.z;

        float leftEdge = worldPoint.x - prefabHalfX;
        float rightEdge = worldPoint.x + prefabHalfX;

        float meshMinX = _platformWorldBounds.center.x - _platformWorldBounds.extents.x;
        float meshMaxX = _platformWorldBounds.center.x + _platformWorldBounds.extents.x;

        if (leftEdge < meshMinX || rightEdge > meshMaxX)
        {
            return false;
        }

        float frontEdge = worldPoint.z - prefabHalfZ;
        float backEdge = worldPoint.z + prefabHalfZ;

        float meshMinZ = _platformWorldBounds.center.z - _platformWorldBounds.extents.z;
        float meshMaxZ = _platformWorldBounds.center.z + _platformWorldBounds.extents.z;

        if (frontEdge < meshMinZ || backEdge > meshMaxZ)
        {
            return false;
        }

        return true;
    }

    private bool TryGetWorldSize(GameObject anyObject, out Bounds bounds)
    {
        if (anyObject == null)
        {
            bounds = default;
            return false;
        }

        GameObject temp = Instantiate(anyObject, Vector3.zero, Quaternion.identity);
        temp.SetActive(true);

        bool result = false;

        try
        {
            Renderer renderer = temp.GetComponentInChildren<Renderer>(true);
            bounds = renderer.bounds;
            result = true;
        }
        catch (System.Exception)
        {
            bounds = default;
        }

        Destroy(temp);
        return result;
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

                var newGradient = new ParticleSystem.MinMaxGradient(color);
                main.startColor = newGradient;
            }
        }

        return UniTask.CompletedTask;
    }

    private void OnDestroy()
    {
        Debug.Log("VoidZoneObjectSpawnerView destroyed");
    }
}