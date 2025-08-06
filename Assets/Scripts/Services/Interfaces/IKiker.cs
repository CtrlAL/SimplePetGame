using Services.EventPublishers;
using System;

namespace Services.Interfaces
{
    public interface IKiker : IDisposable
    {
        void Kick(object sender, KickEventArgs args);
    }
}