using System.Net;

namespace BuoySensorManager.Core.Configuration
{
    public interface IConfig
    {
        /// <summary>
        /// The address of the ECB.
        /// Default is Loopback (127.0.0.1).
        /// </summary>
        IPAddress BuoySensorEcbAddress { get; set; }

        /// <summary>
        /// The networtk port number of the ECB.
        /// Default is 9000.
        /// </summary>
        int BuoySensorEcbPort { get; set; }

        /// <summary>
        /// The number of ports on the ECB.
        /// Default is 4.
        /// </summary>
        int BuoySensorEcbPortCount { get; set; }

        /// <summary>
        /// The time in minutes to wait between buoy packet purges.
        /// Default is 5.
        /// </summary>
        int BuoyPacketPurgeInterval { get; set; }

        /// <summary>
        /// The time in minutes to wait between buoy packet retries.
        /// Default is 5.
        /// </summary>
        int BuoyPacketRetryInterval { get; set; }

        /// <summary>
        /// Sets the name of a buoy by its ECB port number.
        /// Default "Not Set".
        /// </summary>
        string GetBuoyName(int portNumber);

        /// <summary>
        /// Gets the name of a buoy by its ECB port number.
        /// </summary>
        void SetBuoyName(int portNumber, string name);
    }
}