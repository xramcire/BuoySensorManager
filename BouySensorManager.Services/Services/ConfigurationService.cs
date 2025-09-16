using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Services.Models;
using System.Net;

namespace BuoySensorManager.Services.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfig _config;

        public ConfigurationService(IConfig config)
        {
            _config = config;
        }

        public ConfigRequest Get()
        {
            return new ConfigRequest()
            {
                BuoySensorEcbAddress = _config.BuoySensorEcbAddress.ToString(),
                BuoySensorEcbPort = _config.BuoySensorEcbPort,
                BuoyPacketEjectionInterval = _config.BuoyPacketEjectionInterval,
                BuoyPacketPersistDuration = _config.BuoyPacketPersistDuration,
                WaveHeightAlertThreshold = _config.WaveHeightAlertThreshold,
                Buoy0Id = _config.Buoy0Id,
                Buoy1Id = _config.Buoy1Id,
                Buoy2Id = _config.Buoy2Id,
                Buoy3Id = _config.Buoy3Id
            };
        }

        public bool Save(ConfigRequest request)
        {
            //
            //  Form validation validated the form of the IP Address. No need to be pendantic.
            //
            _config.BuoySensorEcbAddress = IPAddress.Parse(request.BuoySensorEcbAddress);
            _config.BuoySensorEcbPort = request.BuoySensorEcbPort;
            _config.BuoyPacketPersistDuration = request.BuoyPacketPersistDuration;
            _config.BuoyPacketEjectionInterval = request.BuoyPacketEjectionInterval;
            _config.WaveHeightAlertThreshold = request.WaveHeightAlertThreshold;
            _config.Buoy0Id = request.Buoy0Id;
            _config.Buoy1Id = request.Buoy1Id;
            _config.Buoy2Id = request.Buoy2Id;
            _config.Buoy3Id = request.Buoy3Id;

            return true;
        }
    }
}
