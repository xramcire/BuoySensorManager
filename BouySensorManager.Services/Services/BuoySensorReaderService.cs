using BuoySensorManager.Core.Configuration;
using BuoySensorManager.Core.Models;
using BuoySensorManager.Core.Repositories;
using BuoySensorManager.Core.Specialized;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Sockets;

namespace BuoySensorManager.Services.Services
{
    public class BuoySensorReaderService : BackgroundService
    {
        private readonly ILogger<BuoySensorReaderService> _logger;
        private readonly IConfig _config;
        private readonly IBuoyPacketRepository _buoyPacketRepository;

        public BuoySensorReaderService(
            ILogger<BuoySensorReaderService> logger,
            IConfig config,
            IBuoyPacketRepository buoyPacketRepository
        )
        {
            _logger = logger;
            _config = config;
            _buoyPacketRepository = buoyPacketRepository;
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
                            for (int i = 0; i < readings.Length; i++)
                            {
                                double depth = readings[i];

                                if (double.IsNaN(depth))
                                {
                                    continue;
                                }

                                await ProcessWaveHeight(i, depth);
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

        /// <summary>
        /// We keep the most recent 600 readings (10 minutes worth)
        /// in memory to create a rolling average of the sea level.
        /// </summary>
        private readonly FixedQueue<double> recentReadings = new(600);

        /// <summary>
        /// This is a low rent substitute for a mapping table.
        /// If there are 1000's of buoys each needs a UID.
        /// </summary>
        private readonly Dictionary<int, string> buoyIds = new()
        {
            { 0, "BUOY-932C2B2B5FC8" },
            { 1, "BUOY-3A367CF75EF3" },
            { 2, "BUOY-40BF8778ADB7" },
            { 3, "BUOY-7A2D0FA8EA74" },
        };

        private async Task ProcessWaveHeight(int port, double depth)
        {
            recentReadings.Add(depth);
            //
            //  We are skipping the previous minute to
            //  avoid unfinished peaks and troughs.
            //
            var oldestReadings = recentReadings.Items.Skip(
                Math.Max(0, recentReadings.Items.Count() - 540)
            );
            //
            //  Sea level is the average of the oldest 9 minutes of depth readings.
            //  It stands to reason the value will be inaccurate until we enough readings.
            //
            var seaLevel = oldestReadings.Average();
            //
            //  Wave amplitude is the difference between the current height and the sea level.
            //
            var amplitude = depth - seaLevel;
            //
            //  Wave height is the absolute value of the amplitude times two.
            //
            var height = Math.Abs(amplitude) * 2;

            BuoyPacket buoyPacket = new()
            {
                BuoyId = buoyIds[port],
                Amplitude = amplitude,
                Depth = depth,
                SeaLevel = seaLevel,
            };

            await _buoyPacketRepository.Create(buoyPacket);

            if (height > 30)
            {
                //
                // TODO: Send Alert.
                //
            }
        }
    }
}
