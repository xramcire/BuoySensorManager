using BuoySensorManager.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using BuoySensorManager.Core;

namespace BuoySensorManager.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services required for the BouyListener application.
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddCore();
            services.AddHostedService<DatabaseManagementService>();
            services.AddHostedService<BuoySensorReaderService>();
            return services;
        }
    }
}
