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

        private IPAddress? buoySensorEcbAddress = null;
        public IPAddress BuoySensorEcbAddress
        {
            get
            {
                string value = _configuration.GetValue(nameof(BuoySensorEcbAddress), IPAddress.Loopback.ToString());
                buoySensorEcbAddress ??= IPAddress.Parse(value);
                return buoySensorEcbAddress!;
            }
            set
            {
                _configuration.SetValue(nameof(BuoySensorEcbAddress), value.ToString());
                buoySensorEcbAddress = value;
            }
        }

        private int? buoySensorEcbPort = null;
        public int BuoySensorEcbPort
        {
            get
            {
                buoySensorEcbPort ??= _configuration.GetValue(nameof(BuoySensorEcbPort), 9000);
                return buoySensorEcbPort.Value;
            }
            set
            {
                _configuration.SetValue(nameof(BuoySensorEcbPort), value);
                buoySensorEcbPort = value;
            }
        }

        private int? waveHeightAlertThreshold = null;
        public int WaveHeightAlertThreshold
        {
            get
            {
                waveHeightAlertThreshold ??= _configuration.GetValue(nameof(WaveHeightAlertThreshold), 30);
                return waveHeightAlertThreshold.Value;
            }
            set
            {
                _configuration.SetValue(nameof(WaveHeightAlertThreshold), value);
                waveHeightAlertThreshold = value;
            }
        }

        private int? buoyPacketPersistDuration = null;
        public int BuoyPacketPersistDuration
        {
            get
            {
                buoyPacketPersistDuration ??= _configuration.GetValue(nameof(BuoyPacketPersistDuration), 60);
                return buoyPacketPersistDuration.Value;
            }
            set
            {
                _configuration.SetValue(nameof(BuoyPacketPersistDuration), value);
                buoyPacketPersistDuration = value;
            }
        }

        private int? buoyPacketEjectionInterval = null;
        public int BuoyPacketEjectionInterval
        {
            get
            {
                buoyPacketEjectionInterval ??= _configuration.GetValue(nameof(BuoyPacketEjectionInterval), 5);
                return buoyPacketEjectionInterval.Value;
            }
            set
            {
                _configuration.SetValue(nameof(BuoyPacketEjectionInterval), value);
                buoyPacketEjectionInterval = value;
            }
        }

        private string? buoy0Id = null;
        public string Buoy0Id
        {
            get
            {
                buoy0Id ??= _configuration.GetValue(nameof(Buoy0Id), "BUOY-XXXX");
                return buoy0Id!;
            }
            set
            {
                _configuration.SetValue(nameof(Buoy0Id), value);
                buoy0Id = value;
            }
        }

        private string? buoy1Id = null;
        public string Buoy1Id
        {
            get
            {
                buoy1Id ??= _configuration.GetValue(nameof(Buoy1Id), "BUOY-XXXX");
                return buoy1Id!;
            }
            set
            {
                _configuration.SetValue(nameof(Buoy1Id), value);
                buoy1Id = value;
            }
        }

        private string? buoy2Id = null;
        public string Buoy2Id
        {
            get
            {
                buoy2Id ??= _configuration.GetValue(nameof(Buoy2Id), "BUOY-XXXX");
                return buoy2Id!;
            }
            set
            {
                _configuration.SetValue(nameof(Buoy2Id), value);
                buoy2Id = value;
            }
        }

        private string? buoy3Id = null;
        public string Buoy3Id
        {
            get
            {
                buoy3Id ??= _configuration.GetValue(nameof(Buoy3Id), "BUOY-XXXX");
                return buoy3Id!;
            }
            set
            {
                _configuration.SetValue(nameof(Buoy3Id), value);
                buoy3Id = value;
            }
        }
    }
}
