using UnityEngine;

namespace Services.Interfaces
{
    public interface IVoidZoneSpawnerService
    {
        void Initialize(Bounds platformBounds, Transform parent);
        void Tick();
        void Cleanup();
    }
}
