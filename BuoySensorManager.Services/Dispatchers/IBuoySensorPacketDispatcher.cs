using BuoySensorManager.Core.Models;

namespace BuoySensorManager.Services.Dispatchers
{
    public interface IBuoySensorPacketDispatcher
    {
        event AsyncBuoyPacketHandler? OnPublished;

        Task Publish(BuoyPacket buoyPacket);
    }
}