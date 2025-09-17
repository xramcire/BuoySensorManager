using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Handlers;
using BuoySensorManager.Core.Repositories;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using SQLitePCL;
using System.Data;

namespace BuoySensorManager.Core
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add core dependencies.
        /// </summary>
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddSingleton<IConfig, Config>();
            //
            //  Batteries??? That is soo intuitive...
            //
            Batteries.Init();

            SqlMapper.AddTypeHandler(new GuidHandler());
            SqlMapper.RemoveTypeMap(typeof(Guid));
            SqlMapper.RemoveTypeMap(typeof(Guid?));

            services.AddSingleton<IDbConnection>(sp =>
            {
                string path = "/data/buoysensormanager.db";

                if (!Directory.Exists("/data"))
                {
                    Directory.CreateDirectory("/data");
                }

                var connectionString = $"Data Source={path}";
                var connection = new SqliteConnection(connectionString);
                connection.Open();
                return connection;
            });

            services.AddSingleton<IBuoyPacketRepository, BuoyPacketRepository>();

            return services;
        }
    }
}
