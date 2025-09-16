using BuoySensorManager.Services.Models;
using BuoySensorManager.Services.Publishers;
using Microsoft.AspNetCore.Components;

namespace BuoySensorManager.Web.Pages
{
    [Route("readings")]
    public partial class Readings
    {
        [Inject]
        private BuoyPacketPublisher Publisher { get; set; } = default!;

        private readonly FixedQueue<BuoyPacketResponse> packets = new(10);

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Publisher.OnPublished += OnPublished;
        }

        private void OnPublished(object? sender, BuoyPacketResponse e)
        {
            packets.Add(e);
            InvokeAsync(StateHasChanged);
        }
    }
}