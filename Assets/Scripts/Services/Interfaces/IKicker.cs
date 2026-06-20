using UnityEngine;

namespace Services.Interfaces
{
    public interface IKiker
    {
        void Kick(GameObject kicked, float kickPower);
    }
}