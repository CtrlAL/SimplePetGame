using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts
{
    public class Fatigue : MonoBehaviour
    {
        [SerializeField] private AbstractStatsSO _stats;
        public float CurrentFatigue { get; private set; } = 0f;

        public void MakeFatigueDamake(int damage)
        {
            if (CurrentFatigue + damage < _stats.Fatigue)
            {
                CurrentFatigue += damage;
            }
        }

        public void Reset(int damage)
        {
            CurrentFatigue = 0f;
        }

        public float GetKnockbackMultiplier(float maxMultiplier = 2f)
        {
            if (_stats.Fatigue <= 0f) return 1f;

            float fatigueRatio = Mathf.Clamp01(CurrentFatigue / _stats.Fatigue);

            return 1f + (fatigueRatio * (maxMultiplier - 1f));
        }
    }
}
