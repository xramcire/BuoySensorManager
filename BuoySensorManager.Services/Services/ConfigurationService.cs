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
            var ret = new ConfigRequest()
            {
                BuoySensorEcbAddress = _config.BuoySensorEcbAddress.ToString(),
                BuoySensorEcbPort = _config.BuoySensorEcbPort,
                BuoySensorEcbPortCount = _config.BuoySensorEcbPortCount,
                BuoyPacketEjectionInterval = _config.BuoyPacketEjectionInterval,
                BuoyPacketPersistDuration = _config.BuoyPacketPersistDuration,
            };

            int count = _config.BuoySensorEcbPortCount;

            for (int port = 0; port < count; port++)
            {
                ret.BuoyNames[port] = _config.GetBuoyName(port);
            }

            return ret;
        }

        public bool Save(ConfigRequest request)
        {
            //
            //  Form validation validated the form of the IP Address. No need to be pendantic.
            //
            _config.BuoySensorEcbAddress = IPAddress.Parse(request.BuoySensorEcbAddress);
            _config.BuoySensorEcbPort = request.BuoySensorEcbPort;
            _config.BuoySensorEcbPortCount = request.BuoySensorEcbPortCount;
            _config.BuoyPacketPersistDuration = request.BuoyPacketPersistDuration;
            _config.BuoyPacketEjectionInterval = request.BuoyPacketEjectionInterval;

            foreach (var kvp in request.BuoyNames)
            {
                _config.SetBuoyName(kvp.Key, kvp.Value);
            }

            return true;
        }
    }
}
