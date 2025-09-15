using BuoySensorManager.Core.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BuoySensorManager.Services.Services
{
    public class DatabaseManagementService : BackgroundService
    {
        private readonly ILogger<DatabaseManagementService> _logger;
        private readonly IBuoyPacketRepository _buoyPacketRepository;

        public DatabaseManagementService(
            ILogger<DatabaseManagementService> logger,
            IBuoyPacketRepository buoyPacketRepository            
        )
        {
            _logger = logger;
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
                //using (var scope = _scopeFactory.CreateScope())
                //{
                //    CleanWaveHeightTable(scope);
                //    //
                //    //  Add other recurring maintenance tasks here...
                //    //
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                //}
            }

            await Task.CompletedTask;
        }

        //private void CleanWaveHeightTable(IServiceScope serviceScope)
        //{
        //    try
        //    {
        //        var config = serviceScope.ServiceProvider.GetRequiredService<IConfig>();

        //        var repository = serviceScope.ServiceProvider.GetRequiredService<IWaveHeightRepository<WaveHeight>>();

        //        var deleteOlderThan = DateTime.UtcNow.AddMinutes(-config.WaveHeightSaveDuration);

        //        repository.Delete(deleteOlderThan);

        //        _logger.LogInformation("{tableName} datatable cleaned.", nameof(WaveHeight));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error cleaning {tableName} datatable.", nameof(WaveHeight));
        //    }
        //}

    }
}
