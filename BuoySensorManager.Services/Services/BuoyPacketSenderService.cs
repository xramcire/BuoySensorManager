using BuoySensorManager.Core.Models;
using BuoySensorManager.Core.Repositories;
using BuoySensorManager.Services.Dispatchers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace BuoySensorManager.Services.Services
{
    public class BuoyPacketSenderService : IHostedService
    {
        private readonly ILogger<BuoyPacketSenderService> _logger;
        private readonly IBuoySensorPacketDispatcher _buoyPacketDispatcher;
        private readonly IBuoyPacketRepository _buoyPacketRepository;
        private readonly HttpClient _buoySensorServer;

        public BuoyPacketSenderService(
            ILogger<BuoyPacketSenderService> logger,
            IBuoySensorPacketDispatcher buoyPacketDispatcher,
            IBuoyPacketRepository buoyPacketRepository,
            IHttpClientFactory httpClientFactory
        )
        {
            _logger = logger;
            _buoyPacketDispatcher = buoyPacketDispatcher;
            _buoyPacketRepository = buoyPacketRepository;
            _buoySensorServer = httpClientFactory.CreateClient("BuoySensorServer");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _buoyPacketDispatcher.OnPublished += async (sender, packet) =>
            {
                await OnPublished(packet, cancellationToken);
            };

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _buoyPacketDispatcher.OnPublished -= async (sender, packet) =>
            {
                await OnPublished(packet, cancellationToken);
            };

            return Task.CompletedTask;
        }

        private async Task OnPublished(BuoyPacket buoyPacket, CancellationToken cancellationToken)
        {
            try
            {
                //
                //  TODO: How do we measure success. http 200, 201???
                //  TODO: Add a constant for the route.
                //
                await PostAsync("api/buoypackets/", buoyPacket, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Packet persisted to the database.");
                await _buoyPacketRepository.Create(buoyPacket);
            }
        }

        private async ValueTask PostAsync(string url, object data, CancellationToken token)
        {
            //
            //  TODO: Authentication...
            //
            string json = JsonSerializer.Serialize(data);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _buoySensorServer.PostAsync(url, requestContent, token);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync(token);
                throw new HttpRequestException(responseContent);
            }
        }
    }
}
