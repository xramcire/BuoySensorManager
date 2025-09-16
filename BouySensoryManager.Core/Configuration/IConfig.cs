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
        /// The port number of the ECB.
        /// Default is 9000.
        /// </summary>
        int BuoySensorEcbPort { get; set; }

        /// <summary>
        /// Wave alert threshold in feet.
        /// Default is 30.
        /// </summary>
        int WaveHeightAlertThreshold { get; set; }

        /// <summary>
        /// The time in minutes to retain saved buoy packets.
        /// Default is 60.
        /// </summary>
        int BuoyPacketPersistDuration { get; set; }

        /// <summary>
        /// The time in minutes to wait between buoy packet ejections.
        /// Default is 5.
        /// </summary>
        int BuoyPacketEjectionInterval { get; set; }
    }
}