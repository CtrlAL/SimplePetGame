using Models;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Services
{
    public class FatigueRestorationService : IFixedTickable
    {
        [Inject] private FatigueModel _model;
        [Inject] private AbstractStats _stats;

        public void FixedTick()
        {
            if (_model.CurrentFatigue.Value > 0)
            {
                float restoration = _stats.FatigueRestoration * Time.deltaTime;
                _model.CurrentFatigue.Value = Mathf.Max(0, _model.CurrentFatigue.Value - restoration);
            }
        }
    }
}
