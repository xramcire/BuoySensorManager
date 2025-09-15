using System.Net;
using System.Net.Sockets;

namespace BuoySensorManager.Ecb
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int port = 9000;
            IPAddress ip = IPAddress.Loopback;

            TcpListener listener = new (ip, port);
            listener.Start();

            Console.WriteLine($"Listening to {ip}:{port}");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                byte[] requestBuffer = new byte[4];
                
                int bytesRead = stream.Read(requestBuffer, 0, requestBuffer.Length);
                
                if (bytesRead < 4)
                {
                    //
                    //  The request was not valid. Must be an int...
                    //
                    client.Close();
                    continue;
                }
                //
                //  This tells us which port (buoy) the consumer wants data for.
                //
                int portNumber = BitConverter.ToInt32(requestBuffer, 0);

                double waveHeight = GetWaveHeight(portNumber);

                byte[] responsePacket = BitConverter.GetBytes(waveHeight);

                stream.Write(responsePacket, 0, responsePacket.Length);

                client.Close();
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

        private static double GetWaveHeight(int portNumber)
        {
            //
            // portNumber is just for demo purposes.
            // We arent going it use it.
            //
            var amplitude = Amplitudes[DateTime.UtcNow.Second];
            var seaLevel = SeaLevels[DateTime.UtcNow.Hour];
            return seaLevel + amplitude;
        }
    }
}
