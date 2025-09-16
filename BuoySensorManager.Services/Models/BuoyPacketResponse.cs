namespace BuoySensorManager.Services.Models
{
    public record BuoyPacketResponse
    {
        public int Port { get; init; } = default!;

        public string BuoyId { get; init; } = default!;

        public double Depth { get; init; }

        public double Amplitude { get; init; }

        public double SeaLevel { get; init; }

        public double WaveHeight { get; init; }

        public DateTime ReadingOn { get; init; }

        public bool IsAlert { get; init; }
    }
}
