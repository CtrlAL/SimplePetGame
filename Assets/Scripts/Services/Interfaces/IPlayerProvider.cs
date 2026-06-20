using UnityEngine;

namespace Services.Interfaces
{
    public interface IPlayerProvider
    {
        GameObject Instance { get; }
    }
}
