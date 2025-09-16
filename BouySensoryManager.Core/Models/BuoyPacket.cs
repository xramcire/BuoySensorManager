namespace BuoySensorManager.Core.Models
{
    public record BuoyPacket
    {
        /// <summary>
        /// The number of the port on the ECB.
        /// </summary>
        public int Port { get; init; } = default!;

        /// <summary>
        /// This is the value that is reported by the buoy.
        /// </summary>
        public double Depth { get; init; }

        /// <summary>
        /// This value is calculated.
        /// </summary>
        public double Amplitude { get; init; }

        /// <summary>
        /// This value is calculated.
        /// </summary>
        public double SeaLevel { get; init; }

        /// <summary>
        /// The time the packet was collected.
        /// </summary>
        public long ReadingOn { get; init; } = DateTime.UtcNow.Ticks;
    }
}
