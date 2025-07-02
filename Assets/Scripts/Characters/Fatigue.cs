using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts
{
    public class Fatigue : MonoBehaviour
    {
        [SerializeField] private AbstractStatsSO _stats;
        public float CurrentFatigue { get; private set; } = 0f;

        public void MakeFatigueDamage(int damage)
        {
            if (CurrentFatigue + damage < _stats.Fatigue)
            {
                CurrentFatigue += damage;
            }

            else
            {
                CurrentFatigue = _stats.Fatigue;
            }
        }

        private void FixedUpdate()
        {
            if (CurrentFatigue - _stats.FatigueRestoration > 0)
            {
                CurrentFatigue-= _stats.FatigueRestoration;
            }
            else
            {
                CurrentFatigue = 0;
            }
        }

        public float GetKnockbackMultiplier(float maxMultiplier = 2f)
        {
            if (_stats.Fatigue <= 0f) return 1f;

            float fatigueRatio = Mathf.Clamp01(CurrentFatigue / _stats.Fatigue);

            return 1f + (fatigueRatio * (maxMultiplier - 1f));
        }
    }
}
