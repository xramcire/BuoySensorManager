using Microsoft.Extensions.Hosting;

namespace BuoySensorManager.Services.Services
{
    public class BuoyPacketRetryService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Scan the DB for any packets that have not been sent.
            // If we can send them to the server. Delete them from the DB.
            return Task.CompletedTask;
        }
    }
}
