using Services.Interfaces;
using UnityEngine;
using Zenject;

public class VoidZoneObjectSpawnerView : MonoBehaviour
{
    [Inject] private IVoidZoneSpawnerService _spawnerService;

    private Renderer _platformRenderer;

    private void Start()
    {
        _platformRenderer = GetComponent<Renderer>();
        _spawnerService.Initialize(_platformRenderer.bounds, transform);
    }

    private void FixedUpdate()
    {
        _spawnerService.Tick();
    }

    private void OnDestroy()
    {
        _spawnerService.Cleanup();
    }
}
