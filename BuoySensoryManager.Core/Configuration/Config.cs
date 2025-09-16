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
                string value = _configuration.GetValue(nameof(BuoySensorEcbAddress), "127.0.0.255");
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

        private int? buoySensorEcbPortCount = null;
        public int BuoySensorEcbPortCount
        {
            get
            {
                buoySensorEcbPortCount ??= _configuration.GetValue(nameof(BuoySensorEcbPortCount), 4);
                return buoySensorEcbPortCount.Value;
            }
            set
            {
                _configuration.SetValue(nameof(BuoySensorEcbPortCount), value);
                buoySensorEcbPortCount = value;
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

        private readonly Dictionary<int, string> portNames = [];

        public string GetBuoyName(int portNumber)
        {
            if (portNames.TryGetValue(portNumber, out string? name))
            {
                return name;
            }

            string portKey = GetPortKey(portNumber);
            name = _configuration.GetValue(portKey, "Not Set");

            portNames[portNumber] = name;

            return name;
        }

        public void SetBuoyName(int portNumber, string name)
        {
            string portKey = GetPortKey(portNumber);
            portNames[portNumber] = name;
            _configuration.SetValue(portKey, name);
        }

        private static string GetPortKey(int portNumber) => $"Port{portNumber}Name";
    }
}
