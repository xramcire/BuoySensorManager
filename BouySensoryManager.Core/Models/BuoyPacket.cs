namespace BuoySensorManager.Core.Models
{
    public record BuoyPacket
    {
        /// <summary>
        /// Auto Incremented.
        /// </summary>
        public int Id { get; init; }

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

        public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    }
}
