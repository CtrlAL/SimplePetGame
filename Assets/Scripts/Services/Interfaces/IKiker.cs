using Services.EventPublishers;
using System;

namespace Services.Interfaces
{
    public interface IKiker : IDisposable
    {
        void Kick(KickEventArgs args);
    }
}