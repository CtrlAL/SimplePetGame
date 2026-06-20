using UnityEngine;

namespace Services.Interfaces
{
    public interface IKicker
    {
        void Kick(GameObject kicked, float kickPower);
    }
}