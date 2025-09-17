using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuoySensorManager.Services.Services
{
    public class BuoyPacketDatabaseService : BackgroundService
    {
        private readonly IConfig _config;
        private readonly ILogger<BuoyPacketDatabaseService> _logger;
        private readonly IBuoyPacketRepository _buoyPacketRepository;

        public BuoyPacketDatabaseService(
            IConfig config,
            ILogger<BuoyPacketDatabaseService> logger,
            IBuoyPacketRepository buoyPacketRepository            
        )
        {
            _config = config;
            _logger = logger;
            _buoyPacketRepository = buoyPacketRepository;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            //
            //  Creates the database.
            //  Any database schema changes would need to be executed here...
            //
            await _buoyPacketRepository.Initialize();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var purgeInterval = _config.BuoyPacketPurgeInterval;
                await Task.Delay(TimeSpan.FromMinutes(purgeInterval), stoppingToken);
                //
                //  Let start up happen then execute after the interval.
                //
                try
                {
                    //
                    //  TODO: Determine strategy for managing database size / disk free space.
                    //
                }
                catch (Exception ex)
                {
                    //
                    // TODO: how do we recover???
                    //
                    _logger.LogError(ex, "Error in BuoyPacketDatabaseService");
                }
            }
        }
    }
}
