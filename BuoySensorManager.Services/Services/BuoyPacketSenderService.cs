using BuoySensorManager.Core.Models;
using BuoySensorManager.Core.Repositories;
using BuoySensorManager.Services.Dispatchers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuoySensorManager.Services.Services
{
    public class BuoyPacketSenderService : IHostedService
    {
        private readonly ILogger<BuoyPacketSenderService> _logger;
        private readonly IBuoySensorPacketDispatcher _buoyPacketDispatcher;
        private readonly IBuoyPacketRepository _buoyPacketRepository;

        public BuoyPacketSenderService(
            ILogger<BuoyPacketSenderService> logger,
            IBuoySensorPacketDispatcher buoyPacketDispatcher,
            IBuoyPacketRepository buoyPacketRepository
        )
        {
            _logger = logger;
            _buoyPacketDispatcher = buoyPacketDispatcher;
            _buoyPacketRepository = buoyPacketRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _buoyPacketDispatcher.OnPublished += async (sender, packet) =>
            {
                await OnPublished(packet);
            };

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _buoyPacketDispatcher.OnPublished -= async (sender, packet) =>
            {
                await OnPublished(packet);
            };

            return Task.CompletedTask;
        }

        private async Task OnPublished(BuoyPacket buoyPacket)
        {
            try
            {
                //
                //  Send how?
                //  Http, Tcp?
                //
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Packet persisted to the database.");
                await _buoyPacketRepository.Create(buoyPacket);
            }
        }
    }
}
