using UnityEngine;

namespace Services.Interfaces
{
    public interface IStunEffectService
    {
        void ShowStunEffect(int id, Transform target);
        void HideStunEffect(int id);
        void UpdateAllPositions();
        void Cleanup();
    }
}
