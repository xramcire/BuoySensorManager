using BuoySensorManager.Services.Models;
using BuoySensorManager.Services.Publishers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuoySensorManager.Services.Services
{
    public class BuoySensorAlertService : IHostedService
    {
        private readonly ILogger<BuoySensorAlertService> _logger;
        private readonly BuoyPacketPublisher _buoyPacketPublisher;

        public BuoySensorAlertService(
            ILogger<BuoySensorAlertService> logger,
            BuoyPacketPublisher buoyPacketPublisher
        )
        {
            _logger = logger;
            _buoyPacketPublisher = buoyPacketPublisher;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _buoyPacketPublisher.OnPublished += OnPublished;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _buoyPacketPublisher.OnPublished -= OnPublished;
            return Task.CompletedTask;
        }

        private void OnPublished(object? sender, BuoyPacketResponse buoyPacket)
        {
            if (buoyPacket.IsAlert == false)
            {
                //
                //  This is fine...
                //
                return;
            }

            _logger.LogWarning("High Wave Alert: {waveHeight:F2}ft", buoyPacket.WaveHeight);

            //
            //  Send alert how?
            //  Email, Text, Http, Tcp?
            //
        }
    }
}
