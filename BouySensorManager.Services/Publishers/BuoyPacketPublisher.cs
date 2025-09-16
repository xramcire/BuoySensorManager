using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Models;
using BuoySensorManager.Services.Models;

namespace BuoySensorManager.Services.Publishers
{
    public class BuoyPacketPublisher
    {
        private readonly IConfig _config;

        public BuoyPacketPublisher(IConfig config)
        {   
            _config = config;
        }

        public event EventHandler<BuoyPacketResponse>? OnPublished;

        public void Publish(BuoyPacket buoyPacket)
        {
            var waveHeight = GetWaveHeight(buoyPacket.Amplitude);

            var response = new BuoyPacketResponse()
            {
                Port = buoyPacket.Port,
                Amplitude = buoyPacket.Amplitude,
                BuoyId = GetBuoyId(buoyPacket.Port),
                Depth = buoyPacket.Depth,
                ReadingOn = new DateTime(buoyPacket.ReadingOn),
                SeaLevel = buoyPacket.SeaLevel,
                WaveHeight = waveHeight,
                IsAlert = (waveHeight > _config.WaveHeightAlertThreshold)
            };

            OnPublished?.Invoke(this, response);
        }

        private string GetBuoyId(int port)
        {
            return port switch
            {
                0 => _config.Buoy0Id,
                1 => _config.Buoy1Id,
                2 => _config.Buoy2Id,
                3 => _config.Buoy3Id,
                _ => "Unknown",
            };
        }

        private static double GetWaveHeight(double amplitude)
        {
            //
            //  Wave height is the absolute value of the amplitude times two.
            //
            return Math.Abs(amplitude) * 2;
        }
    }
}
