using Models;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Presenters
{
    public class FatiguePresenter : ITickable
    {
        [Inject] private FatigueModel _model;
        [Inject] private AbstractStats _stats;

        public void Tick()
        {
            if (_model.CurrentFatigue.Value > 0)
            {
                float restoration = _stats.FatigueRestoration * Time.deltaTime;
                _model.CurrentFatigue.Value = Mathf.Max(0, _model.CurrentFatigue.Value - restoration);
            }
        }
    }
}
