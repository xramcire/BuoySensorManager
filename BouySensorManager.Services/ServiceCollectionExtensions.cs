using BuoySensorManager.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using BuoySensorManager.Core;
using BuoySensorManager.Services.Publishers;

namespace BuoySensorManager.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add service dependencies.
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddCore();
            services.AddSingleton<BuoyPacketPublisher>();
            services.AddHostedService<DatabaseManagementService>();
            services.AddHostedService<BuoySensorReaderService>();
            services.AddHostedService<BuoySensorAlertService>();
            return services;
        }
    }
}
