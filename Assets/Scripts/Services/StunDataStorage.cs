using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
    public static class StunDataStorage
    {
        private static Dictionary<GameObject, string> _oldTags = new();

        public static void StoreOldTag(GameObject go, string tag)
        {
            if (!_oldTags.ContainsKey(go))
            {
                _oldTags.Add(go, tag);
            }
            else
            {
                _oldTags[go] = tag;
            }
        }

        public static bool TryGetOldTag(GameObject go, out string tag)
        {
            return _oldTags.TryGetValue(go, out tag);
        }

        public static void RemoveOldTag(GameObject go)
        {
            if (_oldTags.ContainsKey(go))
            {
                _oldTags.Remove(go);
            }
        }
    }
}