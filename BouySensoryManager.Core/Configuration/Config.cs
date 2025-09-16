using BuoySensorManager.Core.Extensions;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace BuoySensorManager.Core.Configuration
{
    public class Config : IConfig
    {
        private readonly IConfiguration _configuration;

        public Config(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IPAddress BuoySensorEcbAddress
        {
            get => _configuration.GetValue(nameof(BuoySensorEcbAddress), IPAddress.Loopback);
            set => _configuration.SetValue(nameof(BuoySensorEcbAddress), value.ToString());
        }

        public int BuoySensorEcbPort
        {
            get => _configuration.GetValue(nameof(BuoySensorEcbPort), 9000);
            set => _configuration.SetValue(nameof(BuoySensorEcbPort), value);
        }

        public int WaveHeightAlertThreshold
        {
            get => _configuration.GetValue(nameof(WaveHeightAlertThreshold), 30);
            set => _configuration.SetValue(nameof(WaveHeightAlertThreshold), value);
        }

        public int BuoyPacketPersistDuration
        {
            get => _configuration.GetValue(nameof(BuoyPacketPersistDuration), 60);
            set => _configuration.SetValue(nameof(BuoyPacketPersistDuration), value);
        }

        public int BuoyPacketEjectionInterval
        {
            get => _configuration.GetValue(nameof(BuoyPacketEjectionInterval), 5);
            set => _configuration.SetValue(nameof(BuoyPacketEjectionInterval), value);
        }

        public string Buoy0Id
        {
            get => _configuration.GetValue(nameof(Buoy0Id), "BUOY-932C2B2B5FC8");
            set => _configuration.SetValue(nameof(Buoy0Id), value);
        }

        public string Buoy1Id
        {
            get => _configuration.GetValue(nameof(Buoy1Id), "BUOY-3A367CF75EF3");
            set => _configuration.SetValue(nameof(Buoy1Id), value);
        }

        public string Buoy2Id
        {
            get => _configuration.GetValue(nameof(Buoy2Id), "BUOY-40BF8778ADB7");
            set => _configuration.SetValue(nameof(Buoy2Id), value);
        }

        public string Buoy3Id
        {
            get => _configuration.GetValue(nameof(Buoy3Id), "BUOY-7A2D0FA8EA74");
            set => _configuration.SetValue(nameof(Buoy3Id), value);
        }
    }
}
