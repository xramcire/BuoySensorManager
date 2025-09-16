using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Repositories;
using Microsoft.Extensions.Hosting;

namespace BuoySensorManager.Services.Services
{
    public class DatabaseManagementService : BackgroundService
    {
        private readonly IConfig _config;
        private readonly IBuoyPacketRepository _buoyPacketRepository;

        public DatabaseManagementService(
            IConfig config,
            IBuoyPacketRepository buoyPacketRepository            
        )
        {
            _config = config;
            _buoyPacketRepository = buoyPacketRepository;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await _buoyPacketRepository.Initialize();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await BuoyPacketEject();
                var ejectionInterval = _config.BuoyPacketEjectionInterval;
                await Task.Delay(TimeSpan.FromMinutes(ejectionInterval), stoppingToken);
            }

            await Task.CompletedTask;
        }

        private async Task BuoyPacketEject()
        {
            var persistDuration = _config.BuoyPacketPersistDuration;
            var ejectOlderThan = DateTime.UtcNow.AddMinutes(-persistDuration);
            await _buoyPacketRepository.Eject(ejectOlderThan);
        }
    }
}
