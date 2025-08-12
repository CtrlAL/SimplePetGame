using Models;
using ScriptableObjects;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class Fatigue : IFatigue
    {
        [Inject]
        private readonly FatigueModel _model;

        [Inject]
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
