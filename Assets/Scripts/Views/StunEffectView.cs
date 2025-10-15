using Services.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Views
{
    public class StunEffectView
    {
        private readonly IStunEffectPool _effectPool;
        private readonly Dictionary<int, ParticleSystem> _effects = new();
        private readonly Dictionary<int, Transform> _targets = new();

        public StunEffectView(IStunEffectPool effectPool) => _effectPool = effectPool;

        public void ShowStunEffect(int id, Transform target)
        {
            if (_effects.ContainsKey(id)) return;

            var effect = _effectPool.SpawnObject();
            effect.transform.position = target.position + Vector3.up;
            effect.gameObject.SetActive(true);
            effect.Play();

            _effects[id] = effect;
            _targets[id] = target;
        }

        public void HideStunEffect(int id)
        {
            if (_effects.TryGetValue(id, out var effect))
            {
                _effectPool.ReturnToPool(effect);
                _effects.Remove(id);
                _targets.Remove(id);
            }
        }

        public void UpdateAllPositions()
        {
            foreach (var (id, target) in _targets)
                _effects[id].transform.position = target.position + Vector3.up;
        }

        public void Cleanup()
        {
            foreach (var effect in _effects.Values)
                _effectPool.ReturnToPool(effect);

            _effects.Clear();
            _targets.Clear();
        }
    }
}