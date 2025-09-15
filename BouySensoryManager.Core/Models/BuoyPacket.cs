namespace BuoySensorManager.Core.Models
{
    public record BuoyPacket
    {
        /// <summary>
        /// Auto Incremented.
        /// </summary>
        public int Id { get; init; }

        /// <summary>
        /// The UID of the Buoy.
        /// </summary>
        public string BuoyId { get; init; } = default!;

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
        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    }
}
