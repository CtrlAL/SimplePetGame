using Assets.Scripts.Services.Interfaces;
using Models;
using ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public class Fatigue : IFatigue
    {
        private readonly FatigueModel _model;

        private readonly AbstractStatsSO _stats;

        public void MakeFatigueDamage(float damage)
        {
            float newFatigue = _model.CurrentFatigue.Value + damage;
            _model.CurrentFatigue.Value = Mathf.Min(newFatigue, _stats.Fatigue);
        }

        public float GetKnockbackMultiplier(float maxMultiplier = 2f)
        {
            if (_stats.Fatigue <= 0) return 1f;

            float fatigueRatio = _model.CurrentFatigue.Value / _stats.Fatigue;
            return 1f + (fatigueRatio * (maxMultiplier - 1f));
        }
    }
}
