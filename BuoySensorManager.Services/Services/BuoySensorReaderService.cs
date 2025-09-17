using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Models;
using BuoySensorManager.Services.Dispatchers;
using BuoySensorManager.Services.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net.Sockets;

namespace BuoySensorManager.Services.Services
{
    public class BuoySensorReaderService : BackgroundService
    {
        private readonly ILogger<BuoySensorReaderService> _logger;
        private readonly IConfig _config;
        private readonly IBuoySensorPacketDispatcher _buoyPacketPublisher;

        public BuoySensorReaderService(
            ILogger<BuoySensorReaderService> logger,
            IConfig config,
            IBuoySensorPacketDispatcher buoyPacketPublisher
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
                    await GetReadings();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in BuoySensorReaderService");
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }
        }

        private async ValueTask GetReadings()
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

                        await HandingReading(port, depth);
                    }
                }
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

        /// <summary>
        /// We keep the most recent 600 readings (10 minutes worth)
        /// in memory to create a rolling average of the sea level.
        /// </summary>
        private readonly ConcurrentDictionary<int, FixedQueue<double>> recentReadings = [];

        private async ValueTask HandingReading(int port, double depth)
        {
            //
            //  TODO: Consider processing and publishing readings from all ports at once.
            //
            if (!recentReadings.TryGetValue(port, out FixedQueue<double>? portReadings))
            {
                portReadings = new(600);
                recentReadings[port] = portReadings;
            }

            portReadings.Add(depth);

            double? seaLevel;

            if (portReadings.Items.Count() < 60)
            {
                //
                //  Because the portReadings are an in memory collection, when the process
                //  starts up there really isnt enough data to get a good reading.
                //  So we wait 1 minute before trusting the data.
                //
                seaLevel = null;
            }
            else
            {
                seaLevel = portReadings.Items.Average();
            }

            BuoyPacket buoyPacket = new()
            {
                Port = port,
                Depth = depth,
                SeaLevel = seaLevel,
            };

            await _buoyPacketPublisher.Publish(buoyPacket);
        }
    }
}
