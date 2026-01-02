using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Zenject;
using ScriptableObjects;

public class VoidZoneObjectSpawnerView : MonoBehaviour
{
    [Inject] DiContainer _container;
    [Inject] VoidZoneSpawnSettings _settings;

    private List<Vector3> activePositions = new();
    private Mesh mesh;
    private Vector3 meshCenterLocal;
    private Vector3 meshExtentsLocal;

    private void Start()
    {
        var meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("Нет MeshFilter или меша!");
            return;
        }

        mesh = meshFilter.sharedMesh;
        meshCenterLocal = mesh.bounds.center;
        meshExtentsLocal = mesh.bounds.extents;

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        if (Time.timeScale == 0)
        {
            yield return new WaitForSeconds(_settings.SpawnInterval);
        }

        int spawned = 0;
        while (spawned < _settings.SpawnCount)
        {
            if (TrySpawnOne()) spawned++;
            yield return new WaitForSeconds(_settings.SpawnInterval);
        }
    }

    private bool TrySpawnOne()
    {
        float platformTopY = meshCenterLocal.y + meshExtentsLocal.y;

        if (!TryGetPrefabBounds(out Bounds prefabBounds))
        {
            Debug.LogError("Не удалось получить габариты префаба!");
            return false;
        }

        Vector3 prefabSize = prefabBounds.size;
        float prefabHalfX = prefabSize.x * 0.5f;
        float prefabHalfZ = prefabSize.z * 0.5f;

        float platformHalfX = meshExtentsLocal.x;
        float platformHalfZ = meshExtentsLocal.z;

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
                meshCenterLocal.x + Random.Range(-spawnRangeX, spawnRangeX),
                platformTopY,
                meshCenterLocal.z + Random.Range(-spawnRangeZ, spawnRangeZ)
            );

            Vector3 worldPoint = transform.TransformPoint(localPoint) + transform.up * _settings.HeightOffset;

            var objects = Physics.OverlapSphere(worldPoint, _settings.MinDistance);

            if (objects.Length >= 1)
            {
                continue;
            }

            bool tooClose = false;
            foreach (var pos in activePositions)
            {
                if (Vector3.Distance(worldPoint, pos) < _settings.MinDistance)
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

    private bool TryGetPrefabBounds(out Bounds bounds)
    {
        bounds = default;

        var collider = _settings.Prefub.GetComponent<BoxCollider>();
        if (collider != null)
        {
            bounds = collider.bounds;
            Vector3 lossyScale = _settings.Prefub.transform.lossyScale;
            Vector3 center = Vector3.Scale(bounds.center, lossyScale);
            Vector3 size = Vector3.Scale(bounds.size, lossyScale);
            bounds = new Bounds(center, size);
            return true;
        }

        return false;
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, Vector3 pos)
    {
        yield return new WaitForSeconds(_settings.Lifetime);
        if (obj != null) Destroy(obj);
        activePositions.Remove(pos);
    }
}