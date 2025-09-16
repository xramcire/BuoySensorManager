using BuoySensorManager.Core.Models;

namespace BuoySensorManager.Services.Dispatchers
{
    public class BuoySensorPacketDispatcher
    {
        public event AsyncBuoyPacketHandler? OnPublished;

        public async Task Publish(BuoyPacket buoyPacket)
        {
            if (OnPublished is null)
            {
                return;
            }

            var invocationList = OnPublished.GetInvocationList();
            var tasks = invocationList.Select(handler => ((AsyncBuoyPacketHandler)handler)(this, buoyPacket));
            await Task.WhenAll(tasks);
        }
    }

    public delegate Task AsyncBuoyPacketHandler(object sender, BuoyPacket packet);
}
