using Services.Interfaces;
using UnityEngine;

namespace Views
{
    public class PlayerProvider : MonoBehaviour, IPlayerProvider
    {
        public GameObject Instance => gameObject;
    }
}
