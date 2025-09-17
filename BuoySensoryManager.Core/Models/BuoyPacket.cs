namespace BuoySensorManager.Core.Models
{
    public record BuoyPacket
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The number of the port on the ECB.
        /// </summary>
        public int Port { get; init; } = default!;

        /// <summary>
        /// This is the value that is reported by the buoy.
        /// </summary>
        public double Depth { get; init; }

        /// <summary>
        /// This value is calculated based on recent readings.
        /// If the value is null there was insufficent data to get a good reading.
        /// </summary>
        public double? SeaLevel { get; init; }

        /// <summary>
        /// The time the packet was collected.
        /// </summary>
        public long ReadingOn { get; init; } = DateTime.UtcNow.Ticks;
    }
}
