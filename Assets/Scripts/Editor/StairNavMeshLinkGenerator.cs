using Unity.AI.Navigation;
using UnityEditor;
using UnityEngine;
using System.Linq;

public static class StairNavMeshLinkGenerator
{
    [MenuItem("Tools/Generate Stair NavMeshLinks")]
    private static void GenerateLinks()
    {
        var ladders = GameObject.FindGameObjectsWithTag("Environment")
            .Where(go => go.name.StartsWith("Ladder"))
            .Select(go => go.transform)
            .ToArray();

        if (ladders.Length == 0)
        {
            Debug.LogWarning("No Ladder objects found with tag 'Environment'");
            return;
        }

        int totalLinks = 0;
        Undo.SetCurrentGroupName("Generate Stair NavMeshLinks");

        foreach (var ladder in ladders)
        {
            var linkContainer = ladder.Find("NavMeshLinks");
            if (linkContainer != null)
            {
                if (!EditorUtility.DisplayDialog(
                    $"Ladder '{ladder.name}'",
                    $"NavMeshLinks already exist. Regenerate?",
                    "Yes", "Cancel"))
                    continue;

                Undo.DestroyObjectImmediate(linkContainer.gameObject);
            }

            var steps = ladder.GetComponentsInChildren<BoxCollider>()
                .Select(bc => bc.transform)
                .OrderBy(t => t.position.y)
                .ToArray();

            if (steps.Length < 2)
            {
                Debug.LogWarning($"Ladder '{ladder.name}' has fewer than 2 steps, skipping");
                continue;
            }

            var container = new GameObject("NavMeshLinks");
            container.transform.SetParent(ladder);
            container.transform.localPosition = Vector3.zero;
            Undo.RegisterCreatedObjectUndo(container, "Create NavMeshLink container");

            for (int i = 0; i < steps.Length - 1; i++)
            {
                var lower = steps[i];
                var upper = steps[i + 1];

                var lowerTop = lower.position + Vector3.up * 0.5f;
                var upperTop = upper.position + Vector3.up * 0.5f;

                var direction = (upperTop - lowerTop).normalized;
                var distance = Vector3.Distance(lowerTop, upperTop);
                var inset = Mathf.Min(0.15f, distance * 0.25f);

                var startWorld = lowerTop + direction * inset;
                var endWorld = upperTop - direction * inset;

                var midpoint = (startWorld + endWorld) / 2f;

                var linkGO = new GameObject($"Step{i + 1}_to_Step{i + 2}");
                linkGO.transform.SetParent(container.transform);
                linkGO.transform.position = midpoint;

                Undo.RegisterCreatedObjectUndo(linkGO, "Create NavMeshLink");

                var link = linkGO.AddComponent<NavMeshLink>();
                link.startPoint = linkGO.transform.InverseTransformPoint(startWorld);
                link.endPoint = linkGO.transform.InverseTransformPoint(endWorld);
                link.width = 0.8f;
                link.bidirectional = true;
                link.area = 0;
                link.autoUpdate = true;

                totalLinks++;
            }
        }

        Debug.Log($"Created {totalLinks} NavMeshLinks. Bake NavMeshSurface now.");
    }
}
