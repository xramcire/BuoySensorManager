using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace BuoySensorManager.Ecb
{
    internal class Program
    {
        static void Main()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Default", LogLevel.Information).AddConsole();
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();

            int port = 9000;
            IPAddress ip = IPAddress.Parse("127.0.0.255"); //.Loopback;

            TcpListener listener = new (ip, port);
            listener.Start();

            logger.LogInformation("Now listening on: {ip}:{port}", ip, port);

            while (true)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                {
                    using (NetworkStream stream = client.GetStream())
                    {
                        double[] readings = GetReadings();

                        byte[] lengthBytes = BitConverter.GetBytes(readings.Length);
                        byte[] dataBytes = new byte[readings.Length * sizeof(double)];
                        Buffer.BlockCopy(readings, 0, dataBytes, 0, dataBytes.Length);

                        stream.Write(lengthBytes, 0, lengthBytes.Length);
                        stream.Write(dataBytes, 0, dataBytes.Length);
                    }

                    client.Close();
                }
            }
        }

        //
        //  Simulate tidal sea level change over a 24 hour period.
        //
        static readonly int[] SeaLevels = [
            100, 101, 102,
            103, 104, 105,
            106, 107, 108,
            109, 110, 111,
            110, 109, 108,
            107, 106, 105,
            104, 103, 102,
            101, 100, 100,
        ];
        //
        //  Simulate wave amplitude changes over a one minute period.
        //
        static readonly double[] Amplitudes = [
            0.0, 1.5, 3.0, 4.5, 6.0, 7.5, 9.0, 10.5, 12.0, 13.5,
            15.1, 13.5, 12.0, 10.5, 9.0, 7.5, 6.0, 4.5, 3.0, 1.5,
            0.0, -1.5, -3.0, -4.5, -6.0, -7.5, -9.0, -10.5, -12.0, -13.5,
            -15.1, -13.5, -12.0,  -10.5, -9.0, -7.5, -6.0, -4.5, -3.0, -1.5,
            0.0, 1.2, 2.4, 3.6, 4.8, 6.0, 7.2, 8.4, 9.6,  10.8,
            12.0, 10.8, 9.6, 8.4, 7.2, 6.0, 4.8, 3.6, 2.4, 1.2
        ];

        private static double[] GetReadings()
        {
            var amplitude = Amplitudes[DateTime.UtcNow.Second];
            var seaLevel = SeaLevels[DateTime.UtcNow.Hour];
            //
            //  This device has 4 ports.
            //  0 connected
            //  1 disconnected
            //  2 disconnected
            //  3 disconnected
            //
            var depth = seaLevel + amplitude;
            return [depth, double.NaN, double.NaN, double.NaN];
        }
    }
}
