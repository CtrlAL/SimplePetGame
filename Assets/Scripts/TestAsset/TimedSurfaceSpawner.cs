using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Zenject;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class TimedSurfaceSpawner : MonoBehaviour
{
    [Inject] DiContainer _container;

    public GameObject prefabToSpawn;
    public int spawnCount = 10;
    public float spawnInterval = 2f;
    public float lifetime = 10f;
    public float minDistance = 2f;
    public float heightOffset = 0.1f;

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
        int spawned = 0;
        while (spawned < spawnCount)
        {
            if (TrySpawnOne()) spawned++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private bool TrySpawnOne()
    {
        float topY = meshCenterLocal.y + meshExtentsLocal.y;

        for (int attempt = 0; attempt < 100; attempt++)
        {
            Vector3 localPoint = new Vector3(
                Random.Range(meshCenterLocal.x - meshExtentsLocal.x, meshCenterLocal.x + meshExtentsLocal.x),
                topY,
                Random.Range(meshCenterLocal.z - meshExtentsLocal.z, meshCenterLocal.z + meshExtentsLocal.z)
            );

            Vector3 worldPoint = transform.TransformPoint(localPoint) + transform.up * heightOffset;

            bool tooClose = false;
            foreach (var pos in activePositions)
            {
                if (Vector3.Distance(worldPoint, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                continue;
            }

            Quaternion rot = Quaternion.FromToRotation(Vector3.up, transform.up);
            GameObject obj = _container.InstantiatePrefab(prefabToSpawn, worldPoint, rot, transform);
            activePositions.Add(worldPoint);
            StartCoroutine(DestroyAfterDelay(obj, worldPoint));
            return true;
        }

        return false;
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, Vector3 pos)
    {
        yield return new WaitForSeconds(lifetime);
        if (obj != null) Destroy(obj);
        activePositions.Remove(pos);
    }
}