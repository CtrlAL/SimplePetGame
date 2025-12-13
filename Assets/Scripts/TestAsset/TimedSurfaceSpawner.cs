using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
public class TimedSurfaceSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int spawnCount = 10;
    public float spawnInterval = 2f;
    public float lifetime = 10f;
    public float minDistance = 2f;
    public float heightOffset = 0.1f;

    private List<Vector3> activePositions = new();
    private int spawnedSoFar = 0;
    private Coroutine spawnRoutine;

    private void Start()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnOverTime());
    }

    private IEnumerator SpawnOverTime()
    {
        while (spawnedSoFar < spawnCount)
        {
            if (TrySpawnOne())
            {
                spawnedSoFar++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private bool TrySpawnOne()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        if (mesh == null) return false;

        Vector3[] vertices = mesh.vertices;
        Matrix4x4 localToWorld = transform.localToWorldMatrix;

        for (int attempt = 0; attempt < 50; attempt++)
        {
            Vector3 localPoint = vertices[Random.Range(0, vertices.Length)];
            Vector3 worldPoint = localToWorld.MultiplyPoint3x4(localPoint);

            if (!Physics.Raycast(worldPoint + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 10f))
                continue;

            if (hit.collider == null || hit.collider.gameObject != gameObject)
                continue;

            Vector3 spawnPos = hit.point + hit.normal * heightOffset;

            bool tooClose = false;
            foreach (var pos in activePositions)
            {
                if (Vector3.Distance(spawnPos, pos) < minDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                continue;
            }
            

            GameObject instance = Instantiate(prefabToSpawn, spawnPos, gameObject.transform.rotation);
            activePositions.Add(spawnPos);

            StartCoroutine(DestroyAfterDelay(instance, spawnPos));

            return true;
        }

        return false;
    }

    private IEnumerator DestroyAfterDelay(GameObject obj, Vector3 pos)
    {
        yield return new WaitForSeconds(lifetime);

        if (obj != null)
            Destroy(obj);

        if (activePositions.Contains(pos))
        {
            activePositions.Remove(pos);
        }
            
    }

    private void OnDisable()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
        activePositions.Clear();
    }

    private void OnDrawGizmos()
    {
        foreach (var pos in activePositions)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(pos, minDistance * 0.5f);
        }
    }
}