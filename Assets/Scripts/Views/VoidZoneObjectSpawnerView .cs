using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Zenject;
using ScriptableObjects;
using Unity.VisualScripting;
using System.Linq;

public class VoidZoneObjectSpawnerView : MonoBehaviour
{
    [Inject] DiContainer _container;
    [Inject] LevelSettings _levelSettings;
    [Inject] VoidZoneSpawnSettings _settings;

    private List<Vector3> activePositions = new();
    private Renderer _renderer;
    private Bounds _platformWorldBounds;
    private int _currentCount = 0;


    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _platformWorldBounds = _renderer.bounds;

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        if (Time.timeScale == 0 || !_levelSettings.VoidZoneSpawnEnable)
        {
            yield return new WaitForSeconds(_settings.SpawnInterval);
        }

        while (_currentCount <= _settings.SpawnCount)
        {
            if (TrySpawnOne()) _currentCount++;
            yield return new WaitForSeconds(_settings.SpawnInterval);
        }
    }
    
    private bool TrySpawnOne()
    {
        float platformTopY = _renderer.bounds.center.y + _renderer.bounds.extents.y;
        var prefabBounds = GetWorldSize(_settings.Prefub);

        Vector3 prefabSize = prefabBounds.size;
        float prefabHalfX = prefabSize.x * 0.5f;
        float prefabHalfZ = prefabSize.z * 0.5f;

        float platformHalfX = _renderer.bounds.extents.x;
        float platformHalfZ = _renderer.bounds.extents.z;

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
            Vector3 localPoint = new Vector3(
                _renderer.bounds.center.x + Random.Range(-spawnRangeX, spawnRangeX),
                platformTopY,
                _renderer.bounds.center.z + Random.Range(-spawnRangeZ, spawnRangeZ)
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

            //foreach (var elem in objects)
            //{
            //    Debug.DrawRay(worldPoint, elem.transform.position, Color.green, 5f);
            //    Debug.DrawRay(worldPoint, worldPoint + Vector3.up * 5, Color.magenta, 5f);
            //}

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
            GameObject obj = _container.InstantiatePrefab(_settings.Prefub, worldPoint, rot, transform);

            activePositions.Add(worldPoint);
            StartCoroutine(DestroyAfterDelay(obj, worldPoint));
            return true;
        }

        return false;
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, Vector3 pos)
    {
        yield return new WaitForSeconds(_settings.Lifetime);
        if (obj != null) Destroy(obj);
        activePositions.Remove(pos);
        _currentCount--;
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

    private Bounds GetWorldSize(GameObject anyObject)
    {
        GameObject temp = Instantiate(anyObject, Vector3.zero, Quaternion.identity);
        temp.SetActive(true);
        Renderer renderer = temp.GetComponentInChildren<Renderer>(true);

        Destroy(temp);

        return renderer.bounds;
    }
}