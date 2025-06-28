using UnityEngine;

namespace Assets.Scripts
{
    public class Fatigue : MonoBehaviour
    {
        [SerializeField]
        private float _fatigueThreshold = 100f;

        public float FatigueThreshold => _fatigueThreshold;

        public float CurrentFatigue { get; private set; } = 0f;

        public void MakeFatigueDamake(int damage)
        {
            if (CurrentFatigue + damage < _fatigueThreshold)
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
            if (_fatigueThreshold <= 0f) return 1f;

            float fatigueRatio = Mathf.Clamp01(CurrentFatigue / _fatigueThreshold);

            return 1f + (fatigueRatio * (maxMultiplier - 1f));
        }
    }
}
