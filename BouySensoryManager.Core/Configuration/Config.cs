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

        public int WaveHeightSaveDuration
        {
            get => _configuration.GetValue(nameof(WaveHeightSaveDuration), 60);
            set => _configuration.SetValue(nameof(WaveHeightSaveDuration), value);
        }

        public int WaveHeightCleanInterval
        {
            get => _configuration.GetValue(nameof(WaveHeightCleanInterval), 5);
            set => _configuration.SetValue(nameof(WaveHeightCleanInterval), value);
        }
    }
}
