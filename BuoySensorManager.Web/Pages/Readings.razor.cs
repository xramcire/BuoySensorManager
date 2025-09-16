using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Models;
using BuoySensorManager.Services.Dispatchers;
using BuoySensorManager.Services.Models;
using Microsoft.AspNetCore.Components;

namespace BuoySensorManager.Web.Pages
{
    [Route("readings")]
    public partial class Readings : IDisposable
    {
        [Inject]
        private IConfig Config { get; set; } = default!;

        [Inject]
        private IBuoySensorPacketDispatcher Dipatcher { get; set; } = default!;

        private readonly FixedQueue<BuoyPacketResponse> packets = new(10);

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Dipatcher.OnPublished += async (sender, packet) =>
            {
                await OnPublished(packet);
            };
        }

        private async Task OnPublished(BuoyPacket buoyPacket)
        {
            var response = new BuoyPacketResponse()
            {
                Id = buoyPacket.Id,
                Port = buoyPacket.Port,
                BuoyName = Config.GetBuoyName(buoyPacket.Port),
                Depth = buoyPacket.Depth,
                ReadingOn = new DateTime(buoyPacket.ReadingOn),
            };

            packets.Add(response);

            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            Dipatcher.OnPublished -= async (sender, packet) =>
            {
                await OnPublished(packet);
            };

            GC.SuppressFinalize(this);
        }
    }
}