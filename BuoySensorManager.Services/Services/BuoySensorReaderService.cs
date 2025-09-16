using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Models;
using BuoySensorManager.Services.Dispatchers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace BuoySensorManager.Services.Services
{
    public class BuoySensorReaderService : BackgroundService
    {
        private readonly ILogger<BuoySensorReaderService> _logger;
        private readonly IConfig _config;
        private readonly BuoySensorPacketDispatcher _buoyPacketPublisher;

        public BuoySensorReaderService(
            ILogger<BuoySensorReaderService> logger,
            IConfig config,
            BuoySensorPacketDispatcher buoyPacketPublisher
        )
        {
            _logger = logger;
            _config = config;
            _buoyPacketPublisher = buoyPacketPublisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var client = new TcpClient())
                    {
                        client.Connect(_config.BuoySensorEcbAddress, _config.BuoySensorEcbPort);

                        using (NetworkStream stream = client.GetStream())
                        {
                            var readings = ReceiveReadings(stream);
                            //
                            //  The readings are arranged in order by port number.
                            //
                            for (int port = 0; port < readings.Length; port++)
                            {
                                double depth = readings[port];

                                if (double.IsNaN(depth))
                                {
                                    continue;
                                }

                                BuoyPacket buoyPacket = new()
                                {
                                    Port = port,
                                    Depth = depth,
                                };

                                await _buoyPacketPublisher.Publish(buoyPacket);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in BuoySensorReaderService");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private static double[] ReceiveReadings(NetworkStream stream)
        {
            byte[] lengthBytes = new byte[sizeof(int)];
            stream.ReadExactly(lengthBytes, 0, lengthBytes.Length);
            int length = BitConverter.ToInt32(lengthBytes, 0);

            byte[] dataBytes = new byte[length * sizeof(double)];
            stream.ReadExactly(dataBytes, 0, dataBytes.Length);

            double[] ret = new double[length];
            Buffer.BlockCopy(dataBytes, 0, ret, 0, dataBytes.Length);
            return ret;
        }
    }
}
