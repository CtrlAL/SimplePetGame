using UnityEngine;

namespace Services.Interfaces
{
    public interface IKiker
    {
        void Kick(GameObject kicker, GameObject kicked, float kickPower);
    }
}