using BuoySensorManager.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using BuoySensorManager.Core;
using BuoySensorManager.Services.Dispatchers;

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
            services.AddSingleton<IBuoySensorPacketDispatcher, BuoySensorPacketDispatcher>();
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddHostedService<BuoyPacketPurgeService>();
            services.AddHostedService<BuoySensorReaderService>();
            services.AddHostedService<BuoyPacketSenderService>();
            services.AddHostedService<BuoyPacketRetryService>();
            return services;
        }
    }
}
